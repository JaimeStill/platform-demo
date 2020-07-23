using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

using PlatformDemo.Core.Logging;

namespace PlatformDemo.Core.Extensions
{
    public static class CoreExtensions
    {
        private static readonly string urlPattern = "[^a-zA-Z0-9-.]";

        public static string ToGMTString(this DateTime date) => $"{date.ToString()} GMT";

        public static void EnsureDirectoryExists(this string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public static string UrlEncode(this string url)
        {
            var friendlyUrl = Regex.Replace(url, @"\s", "-").ToLower();
            friendlyUrl = Regex.Replace(friendlyUrl, urlPattern, string.Empty);
            return friendlyUrl;
        }

        public static long RandomLong(this int min, int max = 1073741824)
        {
            var r = new Random();
            return (long)(r.Next(min, max));
        }

        static int RandomYear() => new Random().Next(2015, 2025);
        static int RandomMonth() => new Random().Next(1, 13);
        static int RandomDay() => new Random().Next(1, 29);

        public static DateTime RandomDate() => new DateTime(RandomYear(), RandomMonth(), RandomDay());

        public static string UpdateDateTime(
            this string date,
            int years = 0,
            int months = 0,
            int days = 0,
            int hours = 0,
            int minutes = 0
        )
        {
            return DateTime.Parse(date)
                .AddYears(years)
                .AddMonths(months)
                .AddDays(days)
                .AddHours(hours)
                .AddMinutes(minutes)
                .ToGMTString();
        }

        public static string UrlEncode(this string url, string pattern, string replace = "")
        {
            var friendlyUrl = Regex.Replace(url, @"\s", "-").ToLower();
            friendlyUrl = Regex.Replace(friendlyUrl, pattern, replace);
            return friendlyUrl;
        }

        public static string GetExceptionChain(this Exception ex)
        {
            var message = new StringBuilder(ex.Message);

            if (ex.InnerException != null)
            {
                message.AppendLine();
                message.AppendLine(GetExceptionChain(ex.InnerException));
            }

            return message.ToString();
        }

        public static void HandleError(this IApplicationBuilder app, LogProvider logger)
        {
            app.Run(async context =>
            {
                var error = context.Features.Get<IExceptionHandlerFeature>();

                if (error != null)
                {
                    var ex = error.Error;

                    if (ex is AppException)
                    {
                        switch (((AppException)ex).ExceptionType)
                        {
                            case ExceptionType.Authorization:
                                await logger.CreateLog(context, ex, "auth");
                                break;
                            case ExceptionType.Validation:
                                break;
                            default:
                                await logger.CreateLog(context, ex);
                                break;
                        }

                        await context.SendErrorResponse(ex);
                    }
                    else
                    {
                        await logger.CreateLog(context, ex);
                        await context.SendErrorResponse(ex);
                    }
                }
            });
        }

        static async Task SendErrorResponse(this HttpContext context, Exception ex)
        {
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(ex.GetExceptionChain(), Encoding.UTF8);
        }

        public static IEnumerable<T> DistinctBy<T, TIdentity>(this IEnumerable<T> source, Func<T, TIdentity> identitySelector) =>
            source.Distinct(By(identitySelector));

        public static IEqualityComparer<TSource> By<TSource, TIdentity>(Func<TSource, TIdentity> identitySelector) =>
            new DelegateComparer<TSource, TIdentity>(identitySelector);

        public class DelegateComparer<T, TIdentity> : IEqualityComparer<T>
        {
            private readonly Func<T, TIdentity> identitySelector;

            public DelegateComparer(Func<T, TIdentity> identitySelector)
            {
                this.identitySelector = identitySelector;
            }

            public bool Equals(T x, T y) => Equals(identitySelector(x), identitySelector(y));

            public int GetHashCode(T obj) => identitySelector(obj).GetHashCode();
        }
    }
}