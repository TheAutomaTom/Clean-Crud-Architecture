﻿namespace Whether_Advisory.XXX.Domain.Common
{
  public abstract class AuditableEntity
  {
    public string? CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public string? LastModifiedBy { get; set; }
    public DateTime? LastModifiedDate { get; set; }

  }
}
