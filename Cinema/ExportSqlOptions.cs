namespace Cinema
{
    public class ExportSqlOptions
    {
        public string PostgreSQLFolder { get; set; } = string.Empty;
        public string ConnString { get; set; } = string.Empty;
        public string BackupBatName { get; set; } = string.Empty;
        public string RestoreBatName { get; set; } = string.Empty;
        public string RestoreFile { get; set; } = string.Empty;
        public string ResultFile { get; set; } = string.Empty;
    }
}
