/* Source: https://gist.github.com/lamberta/3768814 
 */
function parseFunction(str) {
    if (!str) return void (0);

    var fn_body_idx = str.indexOf('{'),
        fn_body = str.substring(fn_body_idx + 1, str.lastIndexOf('}')),
        fn_declare = str.substring(0, fn_body_idx),
        fn_params = fn_declare.substring(fn_declare.indexOf('(') + 1, fn_declare.lastIndexOf(')')),
        args = fn_params.split(',');

    args.push(fn_body);

    function Fn() {
        return Function.apply(this, args);
    }

    Fn.prototype = Function.prototype;

    return new Fn();
}

window.onload = function () {
    var configObject = JSON.parse('{"urls":[{"url":"/swagger/v1/swagger.json","name":"Sample API"}],"deepLinking":false,"persistAuthorization":false,"displayOperationId":true,"defaultModelsExpandDepth":1,"defaultModelExpandDepth":1,"defaultModelRendering":"example","displayRequestDuration":true,"docExpansion":"none","showExtensions":false,"showCommonExtensions":false,"supportedSubmitMethods":["get","put","post","delete","options","head","patch","trace"],"tryItOutEnabled":false,"tagsSorter":"alpha"}');
    var oauthConfigObject = JSON.parse('{"scopeSeparator":" ","scopes":[],"useBasicAuthenticationWithAccessCodeGrant":false,"usePkceWithAuthorizationCodeGrant":false}');

    // Workaround for https://github.com/swagger-api/swagger-ui/issues/5945
    configObject.urls.forEach(function (item) {
        if (item.url.startsWith("http") || item.url.startsWith("/")) return;
        item.url = window.location.href.replace("index.html", item.url).split('#')[0];
    });

    // If validatorUrl is not explicitly provided, disable the feature by setting to null
    if (!configObject.hasOwnProperty("validatorUrl"))
        configObject.validatorUrl = null

    // If oauth2RedirectUrl isn't specified, use the built-in default
    if (!configObject.hasOwnProperty("oauth2RedirectUrl"))
        configObject.oauth2RedirectUrl = (new URL("oauth2-redirect.html", window.location.href)).href;

    // Apply mandatory parameters
    configObject.dom_id = "#swagger-ui";
    configObject.presets = [SwaggerUIBundle.presets.apis, SwaggerUIStandalonePreset];
    configObject.layout = "StandaloneLayout";

    // Parse and add interceptor functions
    var interceptors = JSON.parse('{}');
    if (interceptors.RequestInterceptorFunction)
        configObject.requestInterceptor = parseFunction(interceptors.RequestInterceptorFunction);
    if (interceptors.ResponseInterceptorFunction)
        configObject.responseInterceptor = parseFunction(interceptors.ResponseInterceptorFunction);

    if (configObject.plugins) {
        configObject.plugins = configObject.plugins.map(eval);
    }

    // Begin Swagger UI call region

    const ui = abp.SwaggerUIBundle(configObject);

    ui.initOAuth(oauthConfigObject);

    // End Swagger UI call region

    window.ui = ui
}
