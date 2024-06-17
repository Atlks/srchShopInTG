const sqlite3 = require('sqlite3').verbose();

require("./corex");

const args = process.argv.slice(2); // 去掉前两个参数
var dbF=urldecode( args[0]);
var sdaveObjstr=file_get_contents(dbF);
prmobj=json_decode(sdaveObjstr);
// 使用示例
const dbFilePath = prmobj['dbf'];
    //'D:\\0prj\\mdsj\\mdsjprj\\bin\\Debug\\net8.0\\mercht商家数据\\缅甸.db';
const sql = 'SELECT * FROM 表格1 ';
const params = [];

executeQuery(dbFilePath, sql, params, (err, rows) => {
    if (err) {
        console.error('Query failed:', err);
        return;
    }

    console.log('----------qryrzt----------');

    //console.log(  JSON.stringify(rows) );
    console.log(writeStringToJsonFile(JSON.stringify(rows,null,3)));

});



const fs = require('fs');
const path = require('path');

function generateFileName() {
    const now = new Date();
    const timestamp = now.toISOString().replace(/[-:.]/g, '');
    const milliseconds = now.getMilliseconds().toString().padStart(3, '0');
    return `file_${timestamp}${milliseconds}.json`;
}

function writeStringToJsonFile(str) {
    if (typeof str !== 'string') {
        throw new Error('Input must be a string');
    }

    const fileName = generateFileName();
    const filePath = path.join(__dirname+"/tmp/", fileName);
    const jsonContent =str;

    fs.writeFileSync(filePath, jsonContent, 'utf8');

  //  console.log(`Data written to file: ${filePath}`);
    return fileName;
}


function executeQuery(dbFilePath, sql, params = [], callback) {
    // 打开数据库
    let db = new sqlite3.Database(dbFilePath, (err) => {
        if (err) {
            console.error('Could not open database:', err.message);
            callback(err);
            return;
        }
        console.log('Connected to the SQLite database.');
    });

    // 执行查询
    db.all(sql, params, (err, rows) => {
        if (err) {
            console.error('Error executing SQL:', err.message);
            callback(err);
            return;
        }
        callback(null, rows);
    });

    // 关闭数据库
    db.close((err) => {
        if (err) {
            //   console.error('Could not close database:', err.message);
        }
        // console.log('Closed the database connection.');
    });
}
