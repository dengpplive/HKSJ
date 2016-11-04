/**
 * 取字符串的长度
 * "你好".len() return 4
 * @param {} 
 * @returns {}
 */
String.prototype.len = function () {
    var len = 0;
    for (var i = 0; i < this.length; i++) {
        var val = this.charAt(i);
        if (val.isChinese()) {
            len += 2;
        } else {
            len += 1;
        }
    }
    return len;
}
/**
 * 比较字符串与length的长度
 * "你好".areBigger(10) return -6
 * @param {} 
 * @returns {}
 */
String.prototype.areBigger = function (length) {
    return this.len() - length;
}
/**
 * 验证是否是中文
 * "你".isChinese() return true
 * @param {}  
 * @returns {}
 */
String.prototype.isChinese = function () {
    var reCh = new RegExp("[\\u4E00-\\u9FFF]+", "g");
    return reCh.test(this);
}
//格式化字符串
String.prototype.Format = function () {
    var result = this;
    if (arguments.length > 0) {
        if (arguments.length == 1 && typeof (args) == "object") {
            for (var key in args) {
                if (args[key] != undefined) {
                    var reg = new RegExp("({" + key + "})", "g");
                    result = result.replace(reg, args[key]);
                }
            }
        }
        else {
            for (var i = 0; i < arguments.length; i++) {
                if (arguments[i] != undefined && arguments[i] != null) {
                    var reg = new RegExp("({[" + i + "]})", "g");
                    result = result.replace(reg, arguments[i]);
                }
            }
        }
    }
    return result;
}