using System;
using System.IO;
using System.Threading.Tasks;
using SQLite;

namespace SQLiteTest.Database
{
    public class TestTable
    {
        [PrimaryKey, AutoIncrement, Column("key")]
        public int Key { get; set; }
        public Guid Id { get; set; }
        public string Text { get; set; }
    }

    public class TestAnotherTable
    {
        [PrimaryKey, AutoIncrement, Column("key")]
        public int Key { get; set; }
        public Guid Id { get; set; }
        public string Text { get; set; }
    }

    public class Database
    {
        public string DatabasePath
        {
            get
            {
                var fileName = "test.db3";
                var docsFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                var libraryPath = Path.Combine(docsFolder, "..", "Library");

                var path = Path.Combine(libraryPath, fileName);

                return path;
            }
        }

        public async Task CreateDatabaseAsync()
        {
            Type[] tableTypes = new Type[]
            {
                typeof (TestTable),
                typeof (TestAnotherTable)
            };


            // Prior to the SQLite-Net-SqlCipher 1.7.335 update.
            // Please comment out the following 4 lines of codes,
            // after updating to SQLite-Net-SqlCipher 1.7.335
            //
            var connection = new SQLiteAsyncConnection(DatabasePath);
            var databasePragma = $"PRAGMA key=xyz";
            var p = await connection.QueryAsync<int>(databasePragma);
            var t = await connection.CreateTablesAsync(CreateFlags.None, tableTypes);

            
            // Manually remove the SQLite-Net-PCL nuget package.
            // Manually remove all the existing SQLitePCLRaw.*.* nuget packages
            // After installing SQLite-Net-SqlCipher 1.7.335 nuget package,
            // please uncomment the following codes, run the app and it will cause
            // 'file is not a database' error.
            //
            //var connString = new SQLiteConnectionString(DatabasePath,
            //                                            storeDateTimeAsTicks: true,
            //                                            key: "xyz",
            //                                            postKeyAction: c =>
            //                                            {
            //                                                c.ExecuteScalar<int>("PRAGMA cipher_migrate");
            //                                            });
            //var connection = new SQLiteAsyncConnection(connString);
            //var t = await connection.CreateTablesAsync(CreateFlags.None, tableTypes);
        }
    }
}
