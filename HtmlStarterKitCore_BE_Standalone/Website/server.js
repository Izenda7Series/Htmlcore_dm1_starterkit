const connect = require('connect');
const serveStatic = require('serve-static');
const port = 8000;
connect().use(serveStatic(__dirname)).listen(port, function(){
    console.log('Server running on port ' + port);
});