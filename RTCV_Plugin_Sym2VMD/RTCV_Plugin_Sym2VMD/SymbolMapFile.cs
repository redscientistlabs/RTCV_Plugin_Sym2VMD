using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RTCV.Common;
using RTCV.CorruptCore;
using RTCV.NetCore;

namespace RTCV_Plugin_Sym2VMD
{
    public enum SymbolMapFormat
    {
        Metrowerks_Wii,
        Metrowerks_GC,
    }
    public enum GenerationMode
    {
        BySourceFile,
        BySourceFile_Alt,
        ByStaticLibrary,
        BySymbol
    }

    public static class SymbolMapFileReader
    {
        public static List<RTCV.CorruptCore.VmdPrototype> GenerateMetrowerksWiiVMDs(TextReader mapfile, GenerationMode mode, MemoryInterface mi, string filter)
        {
            Regex get_source_file = new Regex(@"  [0-9a-f]{8} (?<Size>[0-9a-f]{6}) (?<Address>[0-9a-f]{8}) [ 0-9][0-9] (?<SymbolName>.+) \t(?<SourceFile>.+)", RegexOptions.Compiled);
            Regex get_static_library = new Regex(@"  [0-9a-f]{8} (?<Size>[0-9a-f]{6}) (?<Address>[0-9a-f]{8}) [0-9a-f]{8} [ 0-9][0-9] (?<SymbolName>.+) \t(?<LibraryName>.+) .*", RegexOptions.Compiled);
            Regex get_symbol = new Regex(@"  [0-9a-f]{8} (?<Size>[0-9a-f]{6}) (?<Address>[0-9a-f]{8}) [0-9a-f]{8} [ 0-9][0-9] (?<SymbolName>.+) \t.* .+", RegexOptions.Compiled);
            List<VmdPrototype> vmdPrototypes = new List<VmdPrototype>();
            List<string> filters = filter?.Split('|').ToList();
            string source_file = "";
            string library_name = "";
            string symbol_name = "";
            long starting_address = 0;
            int total_vmd_size = 0;
            // read each line in the map file
            while (true)
            {
                var line = mapfile.ReadLine();
                if (line == null)
                {
                    break;
                }
                switch (mode)
                {
                    case GenerationMode.BySymbol:
                        {
                            MatchCollection matches = get_symbol.Matches(line);
                            foreach (Match match in matches)
                            {
                                GroupCollection groups = match.Groups;
                                if (filters.Count > 0)
                                {
                                    if (!filters.Contains(groups["SymbolName"].Value))
                                    {
                                        continue;
                                    }
                                }
                                var range = new long[2];
                                range[0] = long.Parse(groups["Address"].Value, System.Globalization.NumberStyles.HexNumber) - 0x80000000;
                                range[1] = range[0] + int.Parse(groups["Size"].Value, System.Globalization.NumberStyles.HexNumber);
                                List<long[]> ranges = new List<long[]>();
                                ranges.Add(range);
                                VmdPrototype vmd = new VmdPrototype()
                                {
                                    VmdName = $"{groups["SymbolName"].Value}|{(long.Parse(groups["Address"].Value, System.Globalization.NumberStyles.HexNumber) - 0x80000000):X}",
                                    GenDomain = mi.Name,
                                    BigEndian = mi.BigEndian,
                                    AddRanges = ranges,
                                    WordSize = mi.WordSize,
                                    PointerSpacer = 1,
                                };
                                vmdPrototypes.Add(vmd);
                            }
                            break;
                        }
                    case GenerationMode.ByStaticLibrary:
                        {
                            MatchCollection matches = get_static_library.Matches(line);
                            foreach (Match match in matches)
                            {
                                GroupCollection groups = match.Groups;
                                if (filters.Count > 0)
                                {
                                    if (!filters.Contains(groups["LibraryName"].Value))
                                    {
                                        continue;
                                    }
                                }
                                string _symbol_name = groups["SymbolName"].Value;
                                if (groups["LibraryName"].Value != library_name && library_name != "")
                                {
                                    var range = new long[2];
                                    range[0] = starting_address;
                                    range[1] = starting_address + total_vmd_size;
                                    List<long[]> ranges = new List<long[]>();
                                    ranges.Add(range);
                                    VmdPrototype vmd = new VmdPrototype()
                                    {
                                        VmdName = $"{library_name}|{symbol_name}|{starting_address:X}",
                                        GenDomain = mi.Name,
                                        BigEndian = mi.BigEndian,
                                        AddRanges = ranges,
                                        WordSize = mi.WordSize,
                                        PointerSpacer = 1,
                                    };
                                    starting_address = 0;
                                    total_vmd_size = 0;
                                    symbol_name = "";
                                    library_name = "";
                                    vmdPrototypes.Add(vmd);
                                }
                                if (!(_symbol_name == ".text" || _symbol_name == ".rodata" || _symbol_name == ".data" || _symbol_name == ".sdata" || _symbol_name == ".sdata2" || _symbol_name == ".bss" || _symbol_name == ".sbss" || _symbol_name == ".sbss2"))
                                {
                                    continue;
                                }
                                symbol_name = _symbol_name;
                                long address = long.Parse(groups["Address"].Value, System.Globalization.NumberStyles.HexNumber) - 0x80000000;
                                if (starting_address == 0)
                                {
                                    starting_address = address;
                                }
                                total_vmd_size += int.Parse(groups["Size"].Value, System.Globalization.NumberStyles.HexNumber);
                                library_name = groups["LibraryName"].Value;
                            }
                            break;
                        }
                    case GenerationMode.BySourceFile:
                        {
                            MatchCollection matches = get_source_file.Matches(line);
                            foreach (Match match in matches)
                            {
                                GroupCollection groups = match.Groups;
                                if (filters.Count > 0)
                                {
                                    if (!filters.Contains(groups["SourceFile"].Value.Trim()))
                                    {
                                        continue;
                                    }
                                }
                                string _symbol_name = groups["SymbolName"].Value;
                                if (groups["SourceFile"].Value != source_file && source_file != "")
                                {
                                    var range = new long[2];
                                    range[0] = starting_address;
                                    range[1] = starting_address + total_vmd_size;
                                    List<long[]> ranges = new List<long[]>();
                                    ranges.Add(range);
                                    VmdPrototype vmd = new VmdPrototype()
                                    {
                                        VmdName = $"{source_file}|{symbol_name}|{starting_address:X}",
                                        GenDomain = mi.Name,
                                        BigEndian = mi.BigEndian,
                                        AddRanges = ranges,
                                        WordSize = mi.WordSize,
                                        PointerSpacer = 1,
                                    };
                                    starting_address = 0;
                                    total_vmd_size = 0;
                                    symbol_name = "";
                                    source_file = "";
                                    vmdPrototypes.Add(vmd);
                                }
                                if (!(_symbol_name == ".text" || _symbol_name == ".rodata" || _symbol_name == ".data" || _symbol_name == ".sdata" || _symbol_name == ".sdata2" || _symbol_name == ".bss" || _symbol_name == ".sbss" || _symbol_name == ".sbss2"))
                                {
                                    continue;
                                }
                                symbol_name = _symbol_name;
                                long address = long.Parse(groups["Address"].Value, System.Globalization.NumberStyles.HexNumber) - 0x80000000;
                                if (starting_address == 0)
                                {
                                    starting_address = address;
                                }
                                total_vmd_size += int.Parse(groups["Size"].Value, System.Globalization.NumberStyles.HexNumber);
                                source_file = groups["SourceFile"].Value;
                            }
                            break;
                        }
                    default:
                        break;
                }
            }
            if (vmdPrototypes.Count == 0)
            {
                return null;
            }
            return vmdPrototypes;
        }
        public static List<RTCV.CorruptCore.VmdPrototype> GenerateMetrowerksGCVMDs(TextReader mapfile, GenerationMode mode, MemoryInterface mi, string filter)
        {
            Regex get_source_file = new Regex(@"  [0-9a-f]{8} (?<Size>[0-9a-f]{6}) (?<Address>[0-9a-f]{8}) [ 0-9][0-9] (?<SymbolName>.+) \t(?<SourceFile>.+)", RegexOptions.Compiled);
            Regex get_static_library = new Regex(@"  [0-9a-f]{8} (?<Size>[0-9a-f]{6}) (?<Address>[0-9a-f]{8}) [ 0-9][0-9] (?<SymbolName>.+) \t(?<LibraryName>.+) .*", RegexOptions.Compiled);
            Regex get_symbol = new Regex(@"  [0-9a-f]{8} (?<Size>[0-9a-f]{6}) (?<Address>[0-9a-f]{8}) [ 0-9][0-9] (?<SymbolName>.+).* .+", RegexOptions.Compiled);
            List<VmdPrototype> vmdPrototypes = new List<VmdPrototype>();
            List<string> filters = string.IsNullOrWhiteSpace(filter) ? new List<string>() : filter?.Split('|').ToList();
            string source_file = "";
            string library_name = "";
            string symbol_name = "";
            long starting_address = 0;
            int total_vmd_size = 0;
            // read each line in the map file
            while (true)
            {
                var line = mapfile.ReadLine();
                if (line == null)
                {
                    break;
                }
                switch (mode)
                {
                    case GenerationMode.BySymbol:
                        {
                            MatchCollection matches = get_symbol.Matches(line);
                            foreach (Match match in matches)
                            {
                                GroupCollection groups = match.Groups;
                                if (filters.Count > 0)
                                {
                                    if (!filters.Contains(groups["SymbolName"].Value))
                                    {
                                        continue;
                                    }
                                }
                                var range = new long[2];
                                range[0] = long.Parse(groups["Address"].Value, System.Globalization.NumberStyles.HexNumber) - 0x80000000;
                                range[1] = range[0] + int.Parse(groups["Size"].Value, System.Globalization.NumberStyles.HexNumber);
                                List<long[]> ranges = new List<long[]>();
                                ranges.Add(range);
                                VmdPrototype vmd = new VmdPrototype()
                                {
                                    VmdName = $"{groups["SymbolName"].Value}|{(long.Parse(groups["Address"].Value, System.Globalization.NumberStyles.HexNumber) - 0x80000000):X}",
                                    GenDomain = mi.Name,
                                    BigEndian = mi.BigEndian,
                                    AddRanges = ranges,
                                    WordSize = mi.WordSize,
                                    PointerSpacer = 1,
                                };
                                vmdPrototypes.Add(vmd);
                            }
                            break;
                        }
                    case GenerationMode.ByStaticLibrary:
                        {
                            MatchCollection matches = get_static_library.Matches(line);
                            foreach (Match match in matches)
                            {
                                GroupCollection groups = match.Groups;
                                if (filters.Count > 0)
                                {
                                    if (!filters.Contains(groups["LibraryName"].Value))
                                    {
                                        continue;
                                    }
                                }
                                string _symbol_name = groups["SymbolName"].Value;
                                if (groups["LibraryName"].Value != library_name && library_name != "")
                                {
                                    var range = new long[2];
                                    range[0] = starting_address;
                                    range[1] = starting_address + total_vmd_size;
                                    List<long[]> ranges = new List<long[]>();
                                    ranges.Add(range);
                                    VmdPrototype vmd = new VmdPrototype()
                                    {
                                        VmdName = $"{library_name}|{symbol_name}|{starting_address:X}",
                                        GenDomain = mi.Name,
                                        BigEndian = mi.BigEndian,
                                        AddRanges = ranges,
                                        WordSize = mi.WordSize,
                                        PointerSpacer = 1,
                                    };
                                    starting_address = 0;
                                    total_vmd_size = 0;
                                    symbol_name = "";
                                    library_name = "";
                                    vmdPrototypes.Add(vmd);
                                }
                                if (!(_symbol_name == ".text" || _symbol_name == ".rodata" || _symbol_name == ".data" || _symbol_name == ".sdata" || _symbol_name == ".sdata2" || _symbol_name == ".bss" || _symbol_name == ".sbss" || _symbol_name == ".sbss2"))
                                {
                                    continue;
                                }
                                symbol_name = _symbol_name;
                                long address = long.Parse(groups["Address"].Value, System.Globalization.NumberStyles.HexNumber) - 0x80000000;
                                if (starting_address == 0)
                                {
                                    starting_address = address;
                                }
                                total_vmd_size += int.Parse(groups["Size"].Value, System.Globalization.NumberStyles.HexNumber);
                                library_name = groups["LibraryName"].Value;
                            }
                            break;
                        }
                    case GenerationMode.BySourceFile:
                        {
                            MatchCollection matches = get_source_file.Matches(line);
                            foreach (Match match in matches)
                            {
                                GroupCollection groups = match.Groups;
                                if (filters.Count > 0)
                                {
                                    if (!filters.Contains(groups["SourceFile"].Value))
                                    {
                                        continue;
                                    }
                                }
                                string _symbol_name = groups["SymbolName"].Value;
                                if (groups["SourceFile"].Value != source_file && source_file != "")
                                {
                                    var range = new long[2];
                                    range[0] = starting_address;
                                    range[1] = starting_address + total_vmd_size;
                                    List<long[]> ranges = new List<long[]>();
                                    ranges.Add(range);
                                    VmdPrototype vmd = new VmdPrototype()
                                    {
                                        VmdName = $"{source_file}|{symbol_name}|{starting_address:X}",
                                        GenDomain = mi.Name,
                                        BigEndian = mi.BigEndian,
                                        AddRanges = ranges,
                                        WordSize = mi.WordSize,
                                        PointerSpacer = 1,
                                    };
                                    starting_address = 0;
                                    total_vmd_size = 0;
                                    symbol_name = "";
                                    source_file = "";
                                    vmdPrototypes.Add(vmd);
                                }
                                if (!(_symbol_name == ".text" || _symbol_name == ".rodata" || _symbol_name == ".data" || _symbol_name == ".sdata" || _symbol_name == ".sdata2" || _symbol_name == ".bss" || _symbol_name == ".sbss" || _symbol_name == ".sbss2"))
                                {
                                    continue;
                                }
                                symbol_name = _symbol_name;
                                long address = long.Parse(groups["Address"].Value, System.Globalization.NumberStyles.HexNumber) - 0x80000000;
                                if (starting_address == 0)
                                {
                                    starting_address = address;
                                }
                                total_vmd_size += int.Parse(groups["Size"].Value, System.Globalization.NumberStyles.HexNumber);
                                source_file = groups["SourceFile"].Value;
                                
                            }
                            break;
                        }
                    case GenerationMode.BySourceFile_Alt:
                        {
                            MatchCollection matches = get_source_file.Matches(line);
                            foreach (Match match in matches)
                            {
                                GroupCollection groups = match.Groups;
                                if (filters.Count > 0)
                                {
                                    if (!filters.Contains(groups["SourceFile"].Value))
                                    {
                                        continue;
                                    }
                                }
                                //string _symbol_name = groups["SymbolName"].Value;
                                //if (groups["SourceFile"].Value != source_file && source_file != "")
                                //{
                                //    var range = new long[2];
                                //    range[0] = starting_address;
                                //    range[1] = starting_address + total_vmd_size;
                                //    List<long[]> ranges = new List<long[]>();
                                //    ranges.Add(range);
                                //    VmdPrototype vmd = new VmdPrototype()
                                //    {
                                //        VmdName = $"{source_file}|{symbol_name}|{starting_address:X}",
                                //        GenDomain = mi.Name,
                                //        BigEndian = mi.BigEndian,
                                //        AddRanges = ranges,
                                //        WordSize = mi.WordSize,
                                //        PointerSpacer = 1,
                                //    };
                                //    starting_address = 0;
                                //    total_vmd_size = 0;
                                //    symbol_name = "";
                                //    source_file = "";
                                //    vmdPrototypes.Add(vmd);
                                //}
                                //symbol_name = _symbol_name;
                                //long address = long.Parse(groups["Address"].Value, System.Globalization.NumberStyles.HexNumber) - 0x80000000;
                                //if (starting_address == 0)
                                //{
                                //    starting_address = address;
                                //}
                                //total_vmd_size += int.Parse(groups["Size"].Value, System.Globalization.NumberStyles.HexNumber);
                                //source_file = groups["SourceFile"].Value;
                                var range = new long[2];
                                long address = long.Parse(groups["Address"].Value, System.Globalization.NumberStyles.HexNumber) - 0x80000000;
                                source_file = groups["SourceFile"].Value;
                                range[0] = address;
                                range[1] = address + int.Parse(groups["Size"].Value, System.Globalization.NumberStyles.HexNumber);
                                List<long[]> ranges = new List<long[]>();
                                ranges.Add(range);
                                VmdPrototype vmd = new VmdPrototype()
                                {
                                    VmdName = $"{source_file}|{groups["Address"].Value}",
                                    GenDomain = mi.Name,
                                    BigEndian = mi.BigEndian,
                                    AddRanges = ranges,
                                    WordSize = mi.WordSize,
                                    PointerSpacer = 1,
                                };
                                vmdPrototypes.Add(vmd);
                            }
                            break;
                        }
                    default:
                        break;
                }
            }
            if (vmdPrototypes.Count == 0)
            {
                return null;
            }
            return vmdPrototypes;
        }
        public static List<RTCV.CorruptCore.VmdPrototype> GenerateVMDs(TextReader mapfile, SymbolMapFormat format, GenerationMode mode, string mdname, string filter = null)
        {
            switch (format)
            {
                case SymbolMapFormat.Metrowerks_Wii:
                    return GenerateMetrowerksWiiVMDs(mapfile, mode, MemoryDomains.GetInterface(mdname), filter);
                case SymbolMapFormat.Metrowerks_GC:
                    return GenerateMetrowerksGCVMDs(mapfile, mode, MemoryDomains.GetInterface(mdname), filter);
                default:
                    return null;
            }
        }
    }
}
