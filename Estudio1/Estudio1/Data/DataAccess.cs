using Estudio1.Interfaces;
using Estudio1.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Estudio1.Data
{
    public class DataAccess :IDisposable //para que no deje conexiones abiertas
    {
        SQLiteConnection connection;
        public DataAccess()
        {
            var config = DependencyService.Get<IConfig>();
            connection = new SQLiteConnection(System.IO.Path.Combine(config.DirectoryDB, "Divisas.db3"));
            connection.CreateTable<Cotizacion>(); //si no existe crea la tabla si ya existe la abre
        }
        public void Insert<T>(T model)
        {
            connection.Insert(model);
        }
        public void Update<T>(T model)
        {
            connection.Update(model);
        }
        public void Delete<T>(T model)
        {
            connection.Delete(model);
        }
        //public T First<T>() where T : class
        //{
        //   return connection.Table<T>().FirstOrDefault();
        //}

        //public List<T> GetList<T>() where T : class
        //{
        //    return connection.Table<T>().ToList();
        //}
        //public T Find<T> (int pk) where T:class
        //{
        //    return connection.Table<T>().FirstOrDefault(x => x.GetHashCode() == pk);
        //}
        public void Dispose()
        {
            connection.Dispose();
        }
    }
}
