var CTB = CTB || {};
let _canvasElement;
let _context;
window.addEventListener('resize', () => {
    console.log("resize");
    if (_canvasElement !== undefined) {
        _canvasElement.width = window.innerWidth * 0.8;
        _canvasElement.height = window.innerHeight * 0.8;
        CTB.draw(undefined);
    }
});
document.addEventListener('keydown', (event) => {
    DotNet.invokeMethod("CTB.Client", "CanvasKeyDown", event.keyCode);
});
document.addEventListener('keyup', (event) => {
    DotNet.invokeMethod("CTB.Client", "CanvasKeyUp", event.keyCode);
});
CTB.getUserId = () => {
    let id = "";
    const CatchTheBananaUserId = "CatchTheBananaUserId";
    const searchText = `${CatchTheBananaUserId}=`;
    let startIndex = document.cookie.indexOf(searchText);
    if (startIndex === -1) {
        try {
            const random = window.crypto.getRandomValues(new Uint32Array(4));
            id = random[0].toString(16) + "-" + random[1].toString(16) + "-" + random[2].toString(16) + "-" + random[3].toString(16);
        }
        catch (e) {
            console.log("Secure random number generation is not supported.");
            id = Math.floor(Math.random() * 10000000000).toString();
        }
        document.cookie = `${CatchTheBananaUserId}=${id}; max-age=${3600 * 12}; secure; samesite=strict`;
    }
    else {
        startIndex = startIndex + searchText.length;
        const endIndex = document.cookie.indexOf(";", startIndex);
        if (endIndex === -1) {
            id = document.cookie.substr(startIndex);
        }
        else {
            id = document.cookie.substring(startIndex, endIndex);
        }
    }
    return id;
};
CTB.requestAnimationFrame = (timestamp) => {
    CTB.update(timestamp);
    window.requestAnimationFrame(CTB.requestAnimationFrame.bind(CTB));
};
CTB.initialize = (canvasElement) => {
    console.log("=> initialize");
    _canvasElement = canvasElement;
    _context = _canvasElement.getContext("2d");
    CTB.draw(undefined);
    CTB.requestAnimationFrame(0);
};
CTB.update = (timestamp) => {
};
CTB.draw = (game) => {
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