
global['urldecode']=urldecode
function urldecode(encodedString) {
    try {
        return decodeURIComponent(encodedString.replace(/\+/g, ' '));
    } catch (e) {
        console.error('Error decoding URL:', e);
        return null;
    }
}
global['json_decode']=json_decode
function json_decode(jsonString) {
    try {
        return JSON.parse(jsonString);
    } catch (e) {
        console.error('Error decoding JSON:', e);
        return null;
    }
}


global['gettype']=gettype
function gettype(variable) {
    const type = typeof variable;

    if (type === "object") {
        if (variable === null) {
            return "null";
        } else if (Array.isArray(variable)) {
            return "array";
        } else if (variable instanceof Date) {
            return "date";
        } else if (variable instanceof RegExp) {
            return "regexp";
        } else {
            return "object";
        }
    } else if (type === "function") {
        return "function";
    } else if (type === "undefined") {
        return "undefined";
    } else {
        return type;
    }
}
