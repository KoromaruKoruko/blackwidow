using System;
using System.IO;
using System.Reflection;
using Advdupe;

namespace Advdupe2ToE2HoloCode
{
    internal class Program
    {
        public struct Color
        {
            public Double R, G, B, A;
            public override String ToString() => $"{this.R}, {this.G}, {this.B}, {this.A}";
        }
        private struct Entity
        {
            public Vector Pos;
            public Angle Angle;
            public String Model;
            public Boolean HasColorAndEffects;
            public Color? Color;
            public Double? RenderFX;
            public Boolean HasMaterial;
            public String Material;
        }
        private static void Main(String [ ] args)
        {
            args = new string [ ]
            {
                @"C:\Program Files (x86)\Steam\steamapps\common\GarrysMod\garrysmod\data\advdupe2\bank.txt",
                @"output.txt",
                @"-p"
            };
            if (args.Length < 2)
            {
                Console.WriteLine("usage:" + Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().FullName) + " <InputPath> <OutputPath> [-cp (prints output to console)] [-p (purify code)]");
                Environment.Exit(13); // WinCode :: The data is invalid.
            }
            Boolean PrintOutput = false;
            Boolean Pure = false;
            if (args.Length > 2)
            {
                for (Int32 x = 2; x < args.Length; x++)
                {
                    switch (args [ 2 ])
                    {
                        case "-cp":
                        PrintOutput = true;
                        break;

                        case "-p":
                        Pure = true;
                        break;

                        default:
                        Console.WriteLine("usage:" + Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().FullName) + " <InputPath> <OutputPath> [-cp (prints output to console)] [-p (purify code)]");
                        Environment.Exit(13); // WinCode :: The data is invalid.
                        break;
                    }
                }
            }
            Entity [ ] dupe = GetEntities(new Dupe(args [ 0 ]));
            StreamWriter SW = new StreamWriter(File.OpenWrite(args [ 1 ]));
            void Write(String line)
            {
                if (PrintOutput)
                    Console.WriteLine(line);
                SW.WriteLine(line);
            }
            Int32 HOLOSPAWNSTAGE = 1;
            void WriteEnt(String Base, Entity Ent)
            {
                Write($"{Base}holoCreate({HOLOSPAWNSTAGE}, HOLOPOSITION+vec({Ent.Pos}), vec(1,1,1), ang({Ent.Angle}))");
                Write($"{Base}holoModel({HOLOSPAWNSTAGE}, \"{Ent.Model}\")");
                if (Ent.HasMaterial)
                    Write($"{Base}holoMaterial({HOLOSPAWNSTAGE}, \"{Ent.Material}\")");
                if (Ent.HasColorAndEffects)
                {
                    Write($"{Base}holoColor({HOLOSPAWNSTAGE}, vec4({Ent.Color.Value}))");
                    if (Ent.RenderFX.Value != 0)
                        Write($"{Base}holoRenderFX({HOLOSPAWNSTAGE}, {Ent.RenderFX.Value})");
                }
            }
            if (!Pure)
            {
                Write("@persist HOLOSPAWNSTAGE:number HOLOPOSITION:vector");
                Write("# holo spawn script made using 'Advdupe2ToE2HoloCode'");
                Write("# made by SomeGuyOnTheWeb aka Koromaru");
                Write("# mode:startup_script");
                Write("if(first()|HOLOSPAWNSTAGE)");
                Write("{");
                Write("    switch(HOLOSPAWNSTAGE)");
                Write("    {");
                Write("        case 0,");
                Write("            HOLOSPAWNSTAGE=1");
                Write("            HOLOPOSITION=entity():pos()");
                Write("            runOnTick(1)");
                Write("            break");
                Write("");
                foreach (Entity Ent in dupe)
                {
                    Write($"        case {HOLOSPAWNSTAGE},");
                    WriteEnt("            ", Ent);
                    HOLOSPAWNSTAGE++;
                    Write("            HOLOSPAWNSTAGE=" + HOLOSPAWNSTAGE);
                    Write("            exit()");
                    Write("");
                }
                Write("        default,");
                Write("            HOLOSPAWNSTAGE=0");
                Write("            runOnTick(0)");
                Write("            exit()");
                Write("    }");
                Write("}");
                SW.Flush();
                SW.Close();
            }
            else
            {
                Write("# holo spawn script made using 'Advdupe2ToE2HoloCode'");
                Write("# made by SomeGuyOnTheWeb aka Koromaru");
                Write("# mode:pure");
                foreach (Entity Ent in dupe)
                {
                    WriteEnt("", Ent);
                    HOLOSPAWNSTAGE++;
                    Write("");
                }
            }
        }
        private static Entity [ ] GetEntities(Dupe dupe)
        {
            Table Entities = dupe.DupeData [ "Entities" ].TypeTable;
            Entity [ ] outp = new Entity [ Entities.Count ];
            Int32 i = 0;

            foreach (AdvDupeObject Ent in Entities.Values)
            {
                Entity NewEnt = new Entity();
                Table EntData = Ent.TypeTable;
                Table PropDataPhysics = EntData [ "PhysicsObjects" ].TypeTable [ "0" ].TypeTable;
                NewEnt.Pos = PropDataPhysics [ "Pos" ].TypeVec;
                NewEnt.Angle = PropDataPhysics [ "Angle" ].TypeAng;
                NewEnt.Model = EntData [ "Model" ].TypeString;
                Table Mods = EntData [ "EntityMods" ].TypeTable;
                if (Mods != null)
                {
                    Table colour = Mods [ "colour" ].TypeTable;
                    if (colour != null)
                    {
                        NewEnt.HasColorAndEffects = true;
                        NewEnt.RenderFX = colour [ "RenderFX" ].TypeDouble;
                        // Render mode is ignored due to not being implamentable
                        Table Colour = colour [ "Color" ].TypeTable;
                        NewEnt.Color = new Color()
                        {
                            R = Colour [ "r" ].TypeDouble ?? 0,
                            G = Colour [ "g" ].TypeDouble ?? 0,
                            B = Colour [ "b" ].TypeDouble ?? 0,
                            A = Colour [ "a" ].TypeDouble ?? 0,
                        };
                    }

                    Table material = Mods [ "material" ].TypeTable;
                    if (material != null)
                    {
                        NewEnt.HasMaterial = true;
                        NewEnt.Material = material [ "MaterialOverride" ].TypeString;
                        if (String.IsNullOrWhiteSpace(NewEnt.Material))
                        {
                            NewEnt.HasMaterial = false;
                            NewEnt.Material = null;
                        }
                    }
                }
                outp [ i ] = NewEnt;
                i++;
            }

            return outp;
        }
    }
}
