namespace Estudio1.Data
{
    using Estudio1.Interfaces;
    using Estudio1.Models;
    using SQLite;
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Xamarin.Forms;
    public class DataAccess :IDisposable //para que no deje conexiones abiertas
    {
        SQLiteConnection connection;
        public DataAccess()
        {
            var config = DependencyService.Get<IConfig>();
            connection = new SQLiteConnection(System.IO.Path.Combine(config.DirectoryDB, "Divisas.db3"));
            connection.CreateTable<Cotizacion>(); //si no existe crea la tabla si ya existe la abre
        }
        public string DBInformation()
        {
            return connection.DatabasePath;
        }
        public void Insert<T>(T model)
        {
            connection.Insert(model);
        }
        public int InsertOrUpdate<T>(T model)
        {
            return connection.InsertOrReplace(model);
        }
        public void Update<T>(T model)
        {
            connection.Update(model);
        }
        public void Delete<T>(T model)
        {
            connection.Delete(model);
        }
        public T First<T>() where T : class, new()
        {
            return connection.Table<T>().FirstOrDefault();
        }

        public List<T> GetList<T>() where T : class, new()
        {
            List<T> results = new List<T>();
            results = connection.Table<T>().ToList();

            return results;
        }
        public T Find<T>(int pk) where T : class, new()
        {
            return connection.Table<T>().FirstOrDefault(x => x.GetHashCode() == pk);
        }
        public void Dispose()
        {
            connection.Dispose();
        }
    }
}
