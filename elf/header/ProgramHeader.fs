module Elf.Header.ProgramHeader

(*
    1 (0x1): X - Executable
    2 (0x2): W - Writable
    4 (0x4): R - Readable
*)
type PFlags = 
    | X = 0x1UL
    | W = 0x2UL
    | R = 0x4UL
    | XW = 0x3UL
    | XR = 0x5UL
    | WR = 0x6UL
    | ALL = 0x7UL

type PAlign = 
    |X86 = 0x1000UL

type ProgramHeader =
    { PType: uint64 option
      POffset: uint64 option
      PVAddr: uint64 option
      PFileSize: uint64 option
      PMemSize: int64 option
      PFlags: PFlags option
      PAlign: PAlign option }
    
    static member make(?pType, ?pOffsetm, ?pVAddr, ?pFileSize, ?pMemSize, ?pFlags, ?pAlign) =
        { PType = pType
          POffset =  pOffsetm
          PVAddr = pVAddr
          PFileSize = pFileSize
          PMemSize = pMemSize
          PFlags = pFlags
          PAlign =  pAlign }
