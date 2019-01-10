using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Advdupe;

namespace Advdupe2ToE2HoloCode
{
    internal class Program
    {
        public struct Color
        {
            public Byte R, G, B, A;
            public override String ToString() => $"{this.R}, {this.G}, {this.B}, {this.A}";
        }
        private struct Entity
        {
            public Vector Pos;
            public Angle Angle;
            public String Model;
            public Boolean HasColorAndEffects;
            public Color? Color;
            public Int32? RenderFX;
            public Boolean HasMaterial;
            public String Material;
        }
        private static void Main(String [ ] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("usage:" + Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().FullName) + " <InputPath> <OutputPath> [-p (prints output to console)]");
                Environment.Exit(13); // WinCode :: The data is invalid.
            }
            Boolean PrintOutput = false;
            if (args.Length > 2)
            {
                for (Int32 x = 2; x < args.Length; x++)
                {
                    switch (args [ 2 ])
                    {
                        case "-p":
                        PrintOutput = true;
                        break;

                        default:
                        Console.WriteLine("usage:" + Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().FullName) + " <InputPath> <OutputPath> [-p (prints output to console)]");
                        Environment.Exit(13); // WinCode :: The data is invalid.
                        break;
                    }
                }
            }
            Entity [ ] dupe = GetEntities(GetDupe(args [ 0 ]));
            StreamWriter SW = new StreamWriter(File.OpenWrite(args [ 1 ]));
            void Write(String line)
            {
                if (PrintOutput)
                    Console.WriteLine(line);
                SW.WriteLine(line);
            }
            Int32 HOLOSPAWNSTAGE = 1;
            Write("@persist HOLOSPAWNSTAGE:number HOLOPOSITION:vector");
            Write("# holo spawn script made using 'Advdupe2ToE2HoloCode'");
            Write("# made by SomeGuyOnTheWeb aka Koromaru");
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
                Write($"       case {HOLOSPAWNSTAGE},");
                Write($"           holoCreate({HOLOSPAWNSTAGE}, HOLOPOSITION+vec({Ent.Pos}), vec(1,1,1), ang({Ent.Angle}))");
                Write($"           holoModel({HOLOSPAWNSTAGE}, \"{Ent.Model}\")");
                if (Ent.HasMaterial)
                    Write($"           holoMaterial({HOLOSPAWNSTAGE}, \"{Ent.Material}\")");
                if (Ent.HasColorAndEffects)
                {
                    Write($"           holoColor({HOLOSPAWNSTAGE}, vec4({Ent.Color.Value}))");
                    if (Ent.RenderFX.Value != 0)
                        Write($"           holoRenderFX({HOLOSPAWNSTAGE}, {Ent.RenderFX.Value})");
                }
                HOLOSPAWNSTAGE++;
                Write("           HOLOSPAWNSTAGE=" + HOLOSPAWNSTAGE);
                Write("           exit()");
                Write("");
            }
            Write("       default,");
            Write("           HOLOSPAWNSTAGE=0");
            Write("           runOnTick(0)");
            Write("           exit()");
            Write("    }");
            Write("}");
            SW.Flush();
            SW.Close();
        }
        private static Dupe GetDupe(String InputFile)
        {
            FileStream FFile = null;
            try
            {
                FFile = File.OpenRead(InputFile);
            }
            catch
            {
                Console.WriteLine("Unable to read file!");
                Environment.Exit(110); // WinCode :: The system cannot open the device or file specified.
            }
            Dupe? tmpdupe = null;
            Byte [ ] Header = new Byte [ 5 ];
            FFile.Read(Header, 0, 5);
            Byte Revision = Header [ 4 ];
            if (Header [ 0 ] == 'A' && Header [ 1 ] == 'D' && Header [ 2 ] == '2' && Header [ 3 ] == 'F') // advanced dupe 2
            {
                if (Revision > AdvancedDupe.Revision)
                {
                    Console.WriteLine($"UnSupported AdvDupe Revision! ({Revision} > {AdvancedDupe.Revision}), file cant be decoded!");
                    Environment.Exit(13824); // WinCode :: Invalid header.
                }
                else if (Revision < 1)
                {
                    Console.WriteLine($"Invalid AdvDupe Revision! ({Revision}), file cant be decoded!");
                    Environment.Exit(13824); // WinCode :: Invalid header.
                }
                // load based on revision
                tmpdupe = AdvancedDupe.GetDupe(FFile, Revision);
            }
            else if (Header [ 0 ] == '[' && Header [ 1 ] == 'I' && Header [ 2 ] == 'n' && Header [ 3 ] == 'f') // advanced dupe 1
            {
                tmpdupe = AdvancedDupe.GetDupe(FFile, 0);
            }
            else
            {
                Console.WriteLine("Not AdvDupe File!");
                Environment.Exit(13824); // WinCode :: Invalid header.
            }

            if (tmpdupe.HasValue)
            {
                if (tmpdupe.Value.Success)
                {
                    return tmpdupe.Value;
                }
                else
                {
                    Console.WriteLine("Unknown Error while getting raw dupe!");
                    Environment.Exit(13816);// WinCode :: Unknown error occurred
                }
            }
            else
            {
                Console.WriteLine("Unknown Error while getting raw dupe! (dupe==null)");
                Environment.Exit(13816);// WinCode :: Unknown error occurred
            }
#pragma warning disable CS0618 // Type or member is obsolete
            throw new ExecutionEngineException("this should never be thrown!");
#pragma warning restore CS0618 // Type or member is obsolete
        }
        private static Entity [ ] GetEntities(Dupe dupe)
        {
            Dictionary<String, Object> Entities = (Dictionary<String, Object>)((Dictionary<String, Object>)dupe.MainItem.Data) [ "Entities" ];
            Entity [ ] outp = new Entity [ Entities.Count ];
            Int32 i = 0;

            foreach (Object Ent in Entities.Values)
            {
                Entity NewEnt = new Entity();
                Dictionary<String, Object> EntData = (Dictionary<String, Object>)Ent;
                Dictionary<String, Object> PropDataPhysics = ((Dictionary<String, Object>)((Dictionary<String, Object>)EntData [ "PhysicsObjects" ]) [ "0" ]);
                NewEnt.Pos = (Vector)PropDataPhysics [ "Pos" ];
                NewEnt.Angle = (Angle)PropDataPhysics [ "Angle" ];
                NewEnt.Model = (String)EntData [ "Model" ];
                if (EntData.ContainsKey("EntityMods"))
                {
                    Dictionary<String, Object> Mods = (Dictionary<String, Object>)EntData [ "EntityMods" ];
                    if (Mods.ContainsKey("colour"))
                    {
                        NewEnt.HasColorAndEffects = true;
                        Dictionary<String, Object> colour = (Dictionary<String, Object>)Mods [ "colour" ];
                        NewEnt.RenderFX = (Int32)(Double)colour [ "RenderFX" ];
                        // Render mode is ignored due to not being implamentable
                        Dictionary<String, Object> Colour = (Dictionary<String, Object>)colour [ "Color" ];
                        NewEnt.Color = new Color()
                        {
                            R = (Byte)(Double)Colour [ "r" ],
                            G = (Byte)(Double)Colour [ "g" ],
                            B = (Byte)(Double)Colour [ "b" ],
                            A = (Byte)(Double)Colour [ "a" ],
                        };
                    }
                    if (Mods.ContainsKey("material"))
                    {
                        Dictionary<String, Object> material = (Dictionary<String, Object>)Mods [ "material" ];
                        NewEnt.HasMaterial = true;
                        NewEnt.Material = (String)material [ "MaterialOverride" ];
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
