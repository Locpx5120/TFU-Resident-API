using Dapper;
using System.Text.RegularExpressions;
using TFU_Building_API.Core.Infrastructure.Dapper;

namespace TFU_Building_API.Core.Dapper
{
    public class DapperCommon
    {
        private readonly DapperDbContext context;

        public DapperCommon(DapperDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string query, object param)
        {
            using (var conn = context.CreateConnection())
            {
                return await conn.QueryAsync<T>(query, param);
            }
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string query)
        {
            using (var conn = context.CreateConnection())
            {
                return await conn.QueryAsync<T>(query);
            }
        }

        public async Task<T> QueryFirstAsync<T>(string query, object param)
        {
            using (var conn = context.CreateConnection())
            {
                return await conn.QueryFirstAsync<T>(query, param);
            }
        }

        public async Task<T> QueryFirstAsync<T>(string query)
        {
            using (var conn = context.CreateConnection())
            {
                return await conn.QueryFirstAsync<T>(query);
            }
        }

        public async Task<int> ExecuteAsync(string query, object param)
        {
            using (var conn = context.CreateConnection())
            {
                return await conn.ExecuteAsync(query, param);
            }
        }

        public async Task<int> ExecuteNonParamAsync(string query)
        {
            using (var conn = context.CreateConnection())
            {
                return await conn.ExecuteAsync(query);
            }
        }

        public async Task<T> QueryFirstOrDefaultAsync<T>(string query, object param)
        {
            using (var conn = context.CreateConnection())
            {
                return await conn.QueryFirstOrDefaultAsync<T>(query, param);
            }
        }

        /// <summary>
        /// Filter input prevent SQL Injection
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string SanitizeString(string input)
        {
            string result = input.Trim();

            if (!string.IsNullOrEmpty(result))
            {
                // remove newlines and tabs
                result = Regex.Replace(result, @"\t|\n|\r", "");

                // remove not-supported characters (supported are: numbers, regular letters, hyphens, spaces)
                result = Regex.Replace(result, @"[^\p{L}0-9- ]", "");

                // remove double spaces (also trims)
                result = string.Join(" ", result.Split(' ', StringSplitOptions.RemoveEmptyEntries));
            }

            return result;
        }
    }
}
