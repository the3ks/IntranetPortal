const mysql = require('mysql2/promise');

async function check() {
  try {
    const connection = await mysql.createConnection({
      host: '127.0.0.1',
      user: 'root',
      password: '123',
      database: 'IntranetPortal'
    });
    const [rows] = await connection.query('DESCRIBE Permissions');
    console.log(JSON.stringify(rows, null, 2));
    await connection.end();
  } catch(e) {
    console.error(e);
  }
}
check();
