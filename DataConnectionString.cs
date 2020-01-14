using System.Configuration;
using System.Diagnostics;

[ApplicationScopedSetting]
[DebuggerNonUserCode]
[SpecialSetting(SpecialSetting.ConnectionString)]
[DefaultSettingValue("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\Data.mdf;Integrated Security=True")]
public string DataConnectionString => (string)this["DataConnectionString"];
