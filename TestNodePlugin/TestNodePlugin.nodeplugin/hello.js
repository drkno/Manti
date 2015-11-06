var HelloWorldJS = module.exports = function (platform) {
    platform.Log.Error("IT LOADED!");
};

HelloWorldJS.prototype.HelloWorld = function (someNumber) {
    return "Hello World (node.js) " + someNumber;
};

HelloWorldJS.prototype.Dispose = function() {
};