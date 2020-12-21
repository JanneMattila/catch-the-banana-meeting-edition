var CTB = CTB || {};
declare var DotNet: any;

class CanvasTouch {
    id: number;
    x: number;
    y: number;
}

let _canvasElement: HTMLCanvasElement;
let _context: CanvasRenderingContext2D;
let _dotnetRef: any;
let _leftTouchStart: CanvasTouch = undefined;
let _leftTouchCurrent: CanvasTouch = undefined;
let _rightTouchCurrent: CanvasTouch = undefined;

let _imagesLoaded = 0;
let _imagesToLoad = -1;
const _images = [];

const resizeCanvas = () => {
    if (_canvasElement !== undefined) {
        _canvasElement.width = window.innerWidth * 0.9;
        _canvasElement.height = window.innerHeight * 0.9;
    }
}

window.addEventListener('resize', () => {
    console.log("resize");
    resizeCanvas();
});

document.addEventListener('keydown', (event: KeyboardEvent) => {
    if (_dotnetRef !== undefined) {
        _dotnetRef.invokeMethod("CanvasKeyDown", event.keyCode);
    }
});

document.addEventListener('keyup', (event: KeyboardEvent) => {
    if (_dotnetRef !== undefined) {
        _dotnetRef.invokeMethod("CanvasKeyUp", event.keyCode);
    }
});

const setTouchHandlers = (canvas: HTMLCanvasElement) => {
    canvas.addEventListener('touchstart', (event: TouchEvent) => {
        event.preventDefault();
        for (let i = 0; i < event.changedTouches.length; i++) {
            const touch = event.changedTouches[i];
            if (touch.clientX < canvas.width / 2) {
                _leftTouchStart = {
                    id: touch.identifier,
                    x: touch.clientX,
                    y: touch.clientY
                } as CanvasTouch;
                _leftTouchCurrent = undefined;
            }
            else {
                _rightTouchCurrent = {
                    id: touch.identifier,
                    x: touch.clientX,
                    y: touch.clientY
                } as CanvasTouch;
            }
        }
        if (_dotnetRef !== undefined) {
            _dotnetRef.invokeMethod("CanvasTouch", _leftTouchStart, _leftTouchCurrent, _rightTouchCurrent);
        }
    }, false);
    canvas.addEventListener('touchend', (event: TouchEvent) => {
        event.preventDefault();
        for (let i = 0; i < event.changedTouches.length; i++) {
            const touch = event.changedTouches[i];
            if (touch.clientX < canvas.width / 2) {
                _leftTouchStart = undefined;
                _leftTouchCurrent = undefined;
            }
            else {
                _rightTouchCurrent = undefined;
            }
        }
        if (_dotnetRef !== undefined) {
            _dotnetRef.invokeMethod("CanvasTouch", _leftTouchStart, _leftTouchCurrent, _rightTouchCurrent);
        }
    }, false);

    canvas.addEventListener('touchcancel', (event: TouchEvent) => {
        event.preventDefault();
        for (let i = 0; i < event.changedTouches.length; i++) {
            const touch = event.changedTouches[i];
            if (touch.clientX < canvas.width / 2) {
                _leftTouchStart = undefined;
                _leftTouchCurrent = undefined;
            }
            else {
                _rightTouchCurrent = undefined;
            }
        }

        if (_dotnetRef !== undefined) {
            _dotnetRef.invokeMethod("CanvasTouch", _leftTouchStart, _leftTouchCurrent, _rightTouchCurrent);
        }
    }, false);
    canvas.addEventListener('touchmove', (event: TouchEvent) => {
        event.preventDefault();

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
}

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
        } catch (e) {
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
}

CTB.requestAnimationFrame = (timestamp: number) => {
    if (_dotnetRef !== undefined) {
        _dotnetRef.invokeMethod("GameUpdate", timestamp);
    }
    window.requestAnimationFrame(CTB.requestAnimationFrame.bind(CTB));
}

CTB.initialize = (canvasElement: HTMLCanvasElement, dotnetRef: any): string => {
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
