var CTB = CTB || {};
class CanvasTouch {
}
let _timestamp = 0;
let _canvasElement;
let _context;
let _dotnetRef;
let _leftTouchStart = undefined;
let _leftTouchCurrent = undefined;
let _rightTouchCurrent = undefined;
let _imagesLoaded = 0;
let _imagesToLoad = -1;
const _images = [];
const SHARK_INDEX = 3;
const BANANA_INDEX = 4;
const processFullscreenRequest = (x, y) => {
    if (x < document.body.clientWidth * 0.9 &&
        y > document.body.clientHeight * 0.1) {
        return;
    }
    const d = document;
    const element = d.body;
    if (d.webkitFullscreenEnabled) {
        if (element.webkitRequestFullscreen) {
            if (d.webkitFullscreenElement === null) {
                try {
                    element.webkitRequestFullscreen();
                }
                catch (e) {
                    console.log(e);
                }
            }
            else {
                d.webkitExitFullscreen();
            }
            return;
        }
    }
    else if (d.mozFullscreenEnabled) {
        if (element.mozRequestFullscreen) {
            if (d.mozFullscreenElement === null) {
                try {
                    element.mozRequestFullscreen();
                }
                catch (e) {
                    console.log(e);
                }
            }
            else {
                d.mozExitFullscreen();
            }
            return;
        }
    }
    else if (document.fullscreenEnabled) {
        if (element.requestFullscreen) {
            if (d.fullscreenElement === null) {
                element.requestFullscreen();
            }
            else {
                d.exitFullscreen();
            }
        }
        return;
    }
};
const resizeCanvas = () => {
    if (_canvasElement !== undefined) {
        const element = document.getElementById("game");
        const maxWidth = document.documentElement.clientWidth;
        const maxHeight = document.documentElement.clientHeight;
        _canvasElement.width = 1200;
        _canvasElement.height = 800;
        const aspectRatio = _canvasElement.width / _canvasElement.height;
        const availableWidth = maxWidth * 0.9;
        const availableHeight = maxHeight * 0.9;
        let resizeWidth = availableWidth;
        let resizeHeight = availableWidth / aspectRatio;
        if (availableHeight < resizeHeight) {
            console.log(`Height resized to ${resizeHeight} but space only for ${availableHeight}`);
            resizeHeight = availableHeight;
            resizeWidth = availableHeight * aspectRatio;
            if (availableWidth < resizeWidth) {
                console.log(`Width resized to ${resizeWidth} but space only for ${availableWidth}`);
            }
        }
        element.style.width = `${Math.round(resizeWidth)}px`;
        element.style.height = `${Math.round(resizeHeight)}px`;
    }
};
window.addEventListener('resize', () => {
    console.log("resize");
    resizeCanvas();
});
window.addEventListener('focus', () => {
    console.log("window focus");
});
window.addEventListener('blur', () => {
    console.log("window blur");
    if (_dotnetRef !== undefined) {
        _dotnetRef.invokeMethod("ClearInput");
    }
});
document.addEventListener('keydown', (event) => {
    if (event.keyCode === 70 /* F for fullscreen */) {
        processFullscreenRequest(document.body.clientWidth, document.body.clientHeight);
        return;
    }
    if (_dotnetRef !== undefined && !event.altKey && !event.ctrlKey) {
        _dotnetRef.invokeMethod("CanvasKeyDown", event.keyCode);
    }
});
document.addEventListener('keyup', (event) => {
    if (_dotnetRef !== undefined && !event.altKey && !event.ctrlKey) {
        _dotnetRef.invokeMethod("CanvasKeyUp", event.keyCode);
    }
});
const setTouchHandlers = (canvas) => {
    document.addEventListener('touchstart', (event) => {
        //event.preventDefault();
        for (let i = 0; i < event.changedTouches.length; i++) {
            const touch = event.changedTouches[i];
            //if (touch.clientX < canvas.width / 2) {
            processFullscreenRequest(touch.clientX, touch.clientY);
            _leftTouchStart = {
                id: touch.identifier,
                x: touch.clientX,
                y: touch.clientY
            };
            _leftTouchCurrent = undefined;
            //}
            //else {
            //    _rightTouchCurrent = {
            //        id: touch.identifier,
            //        x: touch.clientX,
            //        y: touch.clientY
            //    } as CanvasTouch;
            //}
        }
        if (_dotnetRef !== undefined) {
            _dotnetRef.invokeMethod("CanvasTouch", _leftTouchStart, _leftTouchCurrent, _rightTouchCurrent);
        }
    }, false);
    document.addEventListener('touchend', (event) => {
        //event.preventDefault();
        for (let i = 0; i < event.changedTouches.length; i++) {
            //const touch = event.changedTouches[i];
            //if (touch.clientX < canvas.width / 2) {
            _leftTouchStart = undefined;
            _leftTouchCurrent = undefined;
            //}
            //else {
            //    _rightTouchCurrent = undefined;
            //}
        }
        if (_dotnetRef !== undefined) {
            _dotnetRef.invokeMethod("CanvasTouch", _leftTouchStart, _leftTouchCurrent, _rightTouchCurrent);
        }
    }, false);
    document.addEventListener('touchcancel', (event) => {
        //event.preventDefault();
        for (let i = 0; i < event.changedTouches.length; i++) {
            //const touch = event.changedTouches[i];
            //if (touch.clientX < canvas.width / 2) {
            _leftTouchStart = undefined;
            _leftTouchCurrent = undefined;
            //}
            //else {
            //    _rightTouchCurrent = undefined;
            //}
        }
        if (_dotnetRef !== undefined) {
            _dotnetRef.invokeMethod("CanvasTouch", _leftTouchStart, _leftTouchCurrent, _rightTouchCurrent);
        }
    }, false);
    document.addEventListener('touchmove', (event) => {
        //event.preventDefault();
        for (let i = 0; i < event.changedTouches.length; i++) {
            const touch = event.changedTouches[i];
            if (_leftTouchStart !== undefined && _leftTouchStart.id === touch.identifier) {
                _leftTouchCurrent = new CanvasTouch();
                _leftTouchCurrent.id = touch.identifier;
                _leftTouchCurrent.x = touch.clientX;
                _leftTouchCurrent.y = touch.clientY;
            }
            else if (_rightTouchCurrent !== undefined && _rightTouchCurrent.id === touch.identifier) {
                _rightTouchCurrent = new CanvasTouch();
                _rightTouchCurrent.id = touch.identifier;
                _rightTouchCurrent.x = touch.clientX;
                _rightTouchCurrent.y = touch.clientY;
            }
        }
        if (_dotnetRef !== undefined) {
            _dotnetRef.invokeMethod("CanvasTouch", _leftTouchStart, _leftTouchCurrent, _rightTouchCurrent);
        }
    }, false);
};
const loadImages = () => {
    const files = [
        "monkey1.png",
        "monkey2.png",
        "monkey3.png",
        "shark.png",
        "banana.png"
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
const drawImage = (img, position) => {
    const x = Math.round(position.x);
    const y = Math.round(position.y);
    if (position.rotation >= Math.PI * 3 / 2 || position.rotation <= Math.PI / 2) {
        _context.save();
        _context.translate(x + img.width / 2, y - img.height / 2);
        _context.scale(-1, 1);
        _context.drawImage(img, 0, 0, img.width, img.height, 0, 0, img.width, img.height);
        _context.restore();
    }
    else {
        _context.drawImage(img, x - img.width / 2, y - img.height / 2);
    }
};
CTB.requestAnimationFrame = (timestamp) => {
    const delta = timestamp - _timestamp;
    _timestamp = timestamp;
    if (_dotnetRef !== undefined) {
        const SPEED_CONSTANT = 0.1;
        _dotnetRef.invokeMethod("GameUpdate", delta * SPEED_CONSTANT);
    }
    window.requestAnimationFrame(CTB.requestAnimationFrame.bind(CTB));
};
CTB.initialize = (canvasElement, dotnetRef) => {
    console.log("=> initialize");
    loadImages();
    _canvasElement = canvasElement;
    setTouchHandlers(_canvasElement);
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
    _context.imageSmoothingEnabled = false;
    _context.fillStyle = "#8e2ec4";
    _context.fillRect(0, 0, _canvasElement.width, _canvasElement.height);
    _context.fill();
    // Some screen resolution references for canvas:
    // Mobile: 697x370 Landscape fullscreen (625 without fullscreen)
    // Desktop: 1350x 772
    const scale = 2;
    _context.scale(scale, scale);
    if (game !== undefined) {
        if (_imagesLoaded === _imagesToLoad) {
            for (let i = 0; i < game.bananas.length; i++) {
                const banana = game.bananas[i];
                drawImage(_images[BANANA_INDEX], banana.position);
            }
            for (let i = 0; i < game.monkeys.length; i++) {
                const monkey = game.monkeys[i];
                drawImage(_images[monkey.ui], monkey.position);
            }
            drawImage(_images[game.me.ui], game.me.position);
            for (let i = 0; i < game.sharks.length; i++) {
                const shark = game.sharks[i];
                drawImage(_images[SHARK_INDEX], shark.position);
            }
        }
    }
    _context.fillStyle = "black";
    _context.fillText(`${_canvasElement.width}x${_canvasElement.height}, scale ${scale}`, 50, 50);
    _context.restore();
};
//# sourceMappingURL=CTB.js.map