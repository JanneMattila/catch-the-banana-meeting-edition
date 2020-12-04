var CTB = CTB || {};
var _canvasElement;
var _context;
window.addEventListener('resize', function () {
    console.log("resize");
    if (_canvasElement !== undefined) {
        _canvasElement.width = window.innerWidth * 0.8;
        _canvasElement.height = window.innerHeight * 0.8;
        CTB.draw(undefined);
    }
});
CTB.initialize = function (canvasElement) {
    console.log("=> initialize");
    _canvasElement = canvasElement;
    _context = _canvasElement.getContext("2d");
    CTB.draw(undefined);
};
CTB.draw = function (game) {
    console.log("=> draw");
    console.log(game);
    if (_context === undefined) {
        return;
    }
    _context.save();
    _context.fillStyle = "#8e2ec4";
    _context.fillRect(0, 0, _canvasElement.width, _canvasElement.height);
    _context.fill();
    _context.restore();
};
//# sourceMappingURL=CTB.js.map