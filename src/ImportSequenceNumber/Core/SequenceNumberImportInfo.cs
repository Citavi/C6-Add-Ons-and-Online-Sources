﻿using SwissAcademic.Citavi;
using System;

namespace SwissAcademic.Addons.ImportSequenceNumberAddon
{
    public class SequenceNumberImportInfo
    {
        public Guid SourceId { get; set; }
        
        public string Number { get; set; }
        
        public Reference TargetReference { get; set; }

        public bool Success { get; set; }
    }
}
