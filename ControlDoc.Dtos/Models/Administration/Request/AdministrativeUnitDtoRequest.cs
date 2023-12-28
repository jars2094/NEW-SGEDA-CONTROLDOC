﻿namespace ControlDoc.Models.Models.Administration.Request
{
    public class AdministrativeUnitDtoRequest
    {
        public int DocumentalVersionId { get; set; }

        public int BossId { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool ActiveState { get; set; }
        public string? CreateUser { get; set; }
    }
}