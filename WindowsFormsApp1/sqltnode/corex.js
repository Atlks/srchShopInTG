
global['urldecode']=urldecode
function urldecode(encodedString) {
    try {
        return decodeURIComponent(encodedString.replace(/\+/g, ' '));
    } catch (e) {
        console.error('Error decoding URL:', e);
        return null;
    }
}

const fs = require('fs');
global['file_get_contents']=file_get_contents
function file_get_contents(filePath) {
    try {
        return fs.readFileSync(filePath, 'utf8');
    } catch (error) {
        console.error('Error reading file:', error);
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
