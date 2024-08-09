console.log(111)

import * as fs from 'fs';

// 解析 INI 文件内容并返回一个 Map
function parseIniFile(filePath: string): Map<string, string> {
    const iniMap = new Map<string, string>();
    const data = fs.readFileSync(filePath, 'utf-8');
    
    // 使用正则表达式分解 INI 文件内容
    const lines = data.split('\n');

    lines.forEach(line => {
        line = line.trim();

        // 跳过空行和注释
        if (line === '' || line.startsWith(';') || line.startsWith('#')) {
            return;
        }

        // 处理键值对
        const [key, value] = line.split('=').map(part => part.trim());
        if (key && value) {
            iniMap.set(key, value);
        }
    });

    return iniMap;
}


// 示例用法
const filePath = `D:\\0prj\\mdsj\\mdsjprj\\cfg\\citycode.ini`;
const iniMap = parseIniFile(filePath);
console.log(iniMap)