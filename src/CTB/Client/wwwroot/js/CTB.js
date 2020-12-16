var CTB = CTB || {};
let _canvasElement;
let _context;
let _dotnetRef;
let _imagesLoaded = 0;
let _imagesToLoad = -1;
const _images = [];
const resizeCanvas = () => {
    if (_canvasElement !== undefined) {
        _canvasElement.width = window.innerWidth * 0.9;
        _canvasElement.height = window.innerHeight * 0.9;
    }
};
window.addEventListener('resize', () => {
    console.log("resize");
    resizeCanvas();
});
document.addEventListener('keydown', (event) => {
    if (_dotnetRef !== undefined) {
        _dotnetRef.invokeMethod("CanvasKeyDown", event.keyCode);
    }
});
document.addEventListener('keyup', (event) => {
    if (_dotnetRef !== undefined) {
        _dotnetRef.invokeMethod("CanvasKeyUp", event.keyCode);
    }
});
const loadImages = () => {
    const files = [
        "monkey1.png",
        "monkey2.png",
        "monkey3.png"
    ];
    _imagesToLoad = files.length;
    for (let i = 0; i < files.length; i++) {
        const file = files[i];
        const img = new Image();
        img.onload = function () {
            _imagesLoaded++;
        };
        img.src = "/images/" + file;
        _images[i] = img;
    }
};
const getPlayerId = () => {
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
    if (_dotnetRef !== undefined) {
        _dotnetRef.invokeMethod("GameUpdate", timestamp);
    }
    window.requestAnimationFrame(CTB.requestAnimationFrame.bind(CTB));
};
CTB.initialize = (canvasElement, dotnetRef) => {
    console.log("=> initialize");
    loadImages();
    _canvasElement = canvasElement;
    _dotnetRef = dotnetRef;
    _context = _canvasElement.getContext("2d");
    resizeCanvas();
    CTB.draw(undefined);
    CTB.requestAnimationFrame(0);
    return getPlayerId();
};
CTB.draw = (game) => {
    if (_context === undefined) {
        return;
    }
    _context.save();
    _context.fillStyle = "#8e2ec4";
    _context.fillRect(0, 0, _canvasElement.width, _canvasElement.height);
    _context.fill();
    if (game !== undefined) {
        if (_imagesLoaded === _imagesToLoad) {
            _context.drawImage(_images[0], game.me.position.x, game.me.position.y);
        }
    }
    _context.restore();
};
//# sourceMappingURL=CTB.js.map