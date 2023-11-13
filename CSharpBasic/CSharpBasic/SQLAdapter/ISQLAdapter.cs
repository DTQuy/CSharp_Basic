using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpBasic.SQLAdapter
{
    public interface ISQLAdapter
    {
        public string ConnectionString { get; set; }


        public string TableName { get; set; }

        /// <summary>
        /// Get list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        List<T> GetData<T>() where T : class, new();

        /// <summary>
        /// Get by id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        T Get<T>(Guid id) where T : class, new();

        /// <summary>
        /// Insert
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        int Insert<T>(T item) where T : class, new();

        /// <summary>
        /// Update
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        int Update<T>(T item) where T : class, new();

        /// <summary>
        /// Delete
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        int Delete<T>(Guid id) where T : class, new();
    }
}