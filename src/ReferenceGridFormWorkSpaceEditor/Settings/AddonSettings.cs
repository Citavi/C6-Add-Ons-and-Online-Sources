﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace SwissAcademic.Addons.ReferenceGridFormWorkSpaceEditor
{
    public class AddonSettings
    {
        #region Constructors

        AddonSettings()
        {
            WorkSpaces = new List<WorkSpace>();
        }

        #endregion

        #region Properties

        [JsonIgnore]
        public static AddonSettings Default { get { return new AddonSettings(); } }

        [JsonProperty]
        public List<WorkSpace> WorkSpaces { get; set; }

        #endregion
    }
}