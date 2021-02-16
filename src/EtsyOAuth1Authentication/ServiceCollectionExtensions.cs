using EtsyOAuth1Authentication.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace EtsyOAuth1Authentication.Services
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add this Etsy Oauth 1.0 scoped service to your DI
        /// </summary>
        /// <param name="keystring">The KEYSTRING value you got after registering an App in your Etsy developer account</param>
        /// <param name="sharedSecret">The SHARED SECRET value you got after registering an App in your Etsy developer account</param>
        public static IServiceCollection AddEtsyOAuthScoped(this IServiceCollection services, string keystring, string sharedSecret)
        {
            return services.AddScoped<IEtsyOauth, EtsyOauth>(_ => new EtsyOauth(keystring, sharedSecret));
        }

        /// <summary>
        /// Add this Etsy Oauth 1.0 transient service to your DI
        /// </summary>
        /// <param name="keystring">The KEYSTRING value you got after registering an App in your Etsy developer account</param>
        /// <param name="sharedSecret">The SHARED SECRET value you got after registering an App in your Etsy developer account</param>
        public static IServiceCollection AddEtsyOAuthTransient(this IServiceCollection services, string keystring, string sharedSecret)
        {
            return services.AddTransient<IEtsyOauth, EtsyOauth>(_ => new EtsyOauth(keystring, sharedSecret));
        }

        /// <summary>
        /// Add this Etsy Oauth 1.0 singleton service to your DI
        /// </summary>
        /// <param name="keystring">The KEYSTRING value you got after registering an App in your Etsy developer account</param>
        /// <param name="sharedSecret">The SHARED SECRET value you got after registering an App in your Etsy developer account</param>
        public static IServiceCollection AddEtsyOAuthSingleton(this IServiceCollection services, string keystring, string sharedSecret)
        {
            return services.AddSingleton<IEtsyOauth, EtsyOauth>(_ => new EtsyOauth(keystring, sharedSecret));
        }
    }
}

