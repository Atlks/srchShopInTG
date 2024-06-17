const sqlite3 = require('sqlite3').verbose();
const { open } = require('sqlite');
const path = require('path');

require("./corex");
var obj={"name":11,"age":222};
//write_row(obj,"db2404.db");
// 获取控制台输入的参数
const args = process.argv.slice(2); // 去掉前两个参数
var dbF=urldecode( args[0]);
var sdaveObjstr=file_get_contents(dbF);
prmobj=json_decode(sdaveObjstr);
write_row( prmobj['saveobj'] ,prmobj['dbf']);

// 创建表格的函数
// 创建表格的函数
async function crtTable(tabl, mapx, dbFileName) {
    const db = await open({
        filename: dbFileName,
        driver: sqlite3.Database
    });

    // 打开数据库连接
    await db.run(`PRAGMA foreign_keys = ON`);

    // 创建表格 SQL
    let sql = `CREATE TABLE IF NOT EXISTS ${tabl} (id TEXT PRIMARY KEY)`;
    await db.run(sql);

    // Type mapping from PHP to SQLite
    const typeMapPHP2sqlt = {
        "integer": 'INT',   "number": 'INT',
        "string": 'TEXT'
    };

    // 遍历 mapx 对象并添加列
    for (const [key, value] of Object.entries(mapx)) {
        try{
            if (key.toLowerCase() === 'id') continue;
            let varType=gettype (value);
            let sqltType = typeMapPHP2sqlt[varType];
            if (sqltType.toLowerCase() !== 'int') sqltType = 'TEXT';
            sql = `ALTER TABLE ${tabl} ADD COLUMN ${key} ${sqltType}`;
            await db.run(sql);
        }catch (e)
        {

        }

    }

    // 关闭数据库连接
    await db.close();
}

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

// 将mapx对象转换为SQL插入语句的函数
function arr_toSqlPrms4insert(mapx) {
    const columns = Object.keys(mapx).join(', ');
    const placeholders = Object.keys(mapx).map(() => '?').join(', ');
    const values = Object.values(mapx);
    return { columns, placeholders, values };
}

// 执行非查询SQL命令的函数
async function cmd_ExecuteNonQuery(db, sql, values) {
    const stmt = await db.prepare(sql);
    const result = await stmt.run(values);
    await stmt.finalize();
    return result.changes;
}

// 保存数据的函数
async function write_row(mapx, dbFileName) {
    const tblx = "表格1";

    console.log("Entering function:", "save", tblx, mapx, dbFileName);

    // 创建表格
     await crtTable(tblx, mapx, dbFileName);

    //if(mapx["id"]!=null)
     // { columns, placeholders, values };
    // 构建SQL插入语句
    const { columns, placeholders, values } = arr_toSqlPrms4insert(mapx);
    const sql = `REPLACE INTO ${tblx} (${columns}) VALUES (${placeholders})`;

    console.log("SQL:", sql);

    const db = await open({
        filename: dbFileName,
        driver: sqlite3.Database
    });

    const ret = await cmd_ExecuteNonQuery(db, sql, values);
    console.log("Return value:", ret);
      marker = "----------marker----------";

    console.log(marker); console.log(ret);
    await db.close();
}
