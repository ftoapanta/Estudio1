namespace Estudio1.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data;
    public class DataService
    {
        public bool DeleteAll<T>() where T : class, new()
        {
            try
            {
                using (var da = new DataAccess())
                {
                    var oldRecords = da.GetList<T>();
                    foreach (var oldRecord in oldRecords)
                    {
                        da.Delete(oldRecord);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return false;
            }
        }
        public T DeleteAllAndInsert<T>(T model) where T : class, new()
        {
            try
            {
                using (var da = new DataAccess())
                {
                    var oldRecords = da.GetList<T>();
                    foreach (var oldRecord in oldRecords)
                    {
                        da.Delete(oldRecord);
                    }
                    da.Insert(model);
                    return model;
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                return model;
            }
        }
        public T InsertOrUpdate<T>(T model) where T : class, new()
        {
            try
            {
                using (var da = new DataAccess())
                {
                    var oldRecord = da.Find<T>(model.GetHashCode());
                    if (oldRecord != null)
                    {
                        da.Update(model);
                    }
                    else
                    {
                        da.Insert(model);
                    }
                    return model;
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                return model;
            }
        }
        public T Insert<T>(T model)
        {
            using (var da = new DataAccess())
            {
                da.Insert(model);
                return model;
            }
        }
        public T Find<T>(int pk) where T : class, new()
        {
            using (var da = new DataAccess())
            {
                return da.Find<T>(pk);
            }
        }
        public T First<T>() where T : class, new()
        {
            using (var da = new DataAccess())
            {
                return da.GetList<T>().FirstOrDefault();
            }
        }
        public List<T> Get<T>() where T : class, new()
        {
            using (var da = new DataAccess())
            {
                return da.GetList<T>().ToList();
            }
        }
        public void Update<T>(T model)
        {
            using (var da = new DataAccess())
            {
                da.Update(model);
            }
        }
        public void Delete<T>(T model)
        {
            using (var da = new DataAccess())
            {
                da.Delete(model);
            }
        }
        public void Save<T>(List<T> list) where T : class, new()
        {
            using (var da = new DataAccess())
            {
                foreach (var record in list)
                {
                    InsertOrUpdate(record);
                }
            }
        }
    }
}