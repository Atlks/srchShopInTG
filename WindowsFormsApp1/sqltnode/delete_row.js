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
delete_row( prmobj['buf_row'] ,prmobj['dbf']);


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



// 执行非查询SQL命令的函数
async function cmd_ExecuteNonQuery(db, sql, values) {
    const stmt = await db.prepare(sql);
    const result = await stmt.run(values);
    await stmt.finalize();
    return result.changes;
}

// 保存数据的函数
async function delete_row(mapx, dbFileName) {
    const tblx = "表格1";

    console.log("Entering function:", "save", tblx, mapx, dbFileName);



    //if(mapx["id"]!=null)
     // { columns, placeholders, values };

    const sql = ` delete from ${tblx} where id='${mapx.id}' `;

    console.log("SQL:", sql);

    const db = await open({
        filename: dbFileName,
        driver: sqlite3.Database
    });

    values=[];
    const ret = await cmd_ExecuteNonQuery(db, sql, values);
    console.log("Return value:", ret);
      marker = "----------marker----------";

    console.log(marker); console.log(ret);
    await db.close();
}
