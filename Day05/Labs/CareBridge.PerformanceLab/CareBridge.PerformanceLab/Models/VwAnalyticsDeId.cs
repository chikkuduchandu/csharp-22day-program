using System;
using System.Collections.Generic;

namespace CareBridge.PerformanceLab.Models;

public partial class VwAnalyticsDeId
{
    public int EncounterId { get; set; }

    public string AgeBand { get; set; } = null!;

    public string Gender { get; set; } = null!;

    public string EncounterType { get; set; } = null!;

    public int DepartmentId { get; set; }
}
