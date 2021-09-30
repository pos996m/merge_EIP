function snykebtn() {
    Swal.fire({
        showConfirmButton: false,
        html: `<div id="phaser"></div>`,
        showCloseButton: true
    })
    snykegame();
}

function snykegame() {

    // 主體
    var cat;
    // 食物
    var food;
    // 鍵盤
    var keyboard;
    // 點
    let pointer;
    // 計數器
    let timer;
    // 時間
    let timeInt = 0;
    // 分數
    let sorceInt = 0;
    // 方向判斷
    let x = 1;
    let y = 0;
    // 解決觸碰連點
    var cnt = 0;
    // 遊戲尺寸
    const w = 640;
    const h = 500;
    // 主體與食物的大小
    const block = 20;
    // 寬與高的格數限制
    const weightBlock = 32; //32
    const hightBlock = 22; //22
    // 進入控制器
    let sorceTrue = true;


    // 遊戲開始畫面
    const gameStar = {
        key: 'gameStar',
        preload: function () {
            this.load.image('bg', '/Content/images/cateat/bg_2.png')
            this.load.image('start_title', '/Content/images/cateat/start_2.png');
            this.load.image('start_click', '/Content/images/cateat/startgame_2.png');
        },
        create: function () {
            this.bg = this.add.image(340, 240, 'bg')
            this.start_title = this.add.image(320, 240, 'start_title')
            this.start_click = this.add.image(320, 240, 'start_click')
            this.start_click.setInteractive();
            this.start_click.on('pointerdown', () => this.scene.start('gamePlay'));
        },
        update: function () {

        }
    }
    // 遊戲畫面
    const gamePlay = {
        key: 'gamePlay',
        preload: function () {
            this.load.image('cat', '/Content/images/cateat/cat_2.png');
            this.load.image('love', '/Content/images/cateat/love_2.png');
            this.load.image('bg', '/Content/images/cateat/bg_2.png')
            this.load.image('over_title', '/Content/images/cateat/over_2.png');
            this.load.image('try_again', '/Content/images/cateat/tryagain.png');
            this.load.image('back_menu', '/Content/images/cateat/backmenu.png');
            this.load.audio('eat', '/Content/images/gameAudio/eat.mp3');
        },
        create: function () {
            this.bg = this.add.image(340, 240, 'bg')
            var Love = new Phaser.Class({

                Extends: Phaser.GameObjects.Image,

                initialize:

                    function Love(scene, x, y) {
                        Phaser.GameObjects.Image.call(this, scene)

                        this.setTexture('love');
                        this.setPosition(x * block, y * block);
                        this.setOrigin(0);

                        this.total = 3;

                        scene.children.add(this);
                    },

                eat: function () {
                    sorceInt += 100;
                    this.total++;

                }

            });

            var Cat = new Phaser.Class({

                initialize:

                    function Cat(scene, x, y) {
                        this.headPosition = new Phaser.Geom.Point(x, y);
                        this.body = scene.add.group();
                        this.head = this.body.create(x * block, y * block, 'cat');
                        this.head.setOrigin(0);
                        this.alive = true;
                        this.speed = 100;
                        this.moveTime = 0;
                        this.tail = new Phaser.Geom.Point(x, y);
                    },

                update: function (time) {
                    if (time >= this.moveTime) {
                        return this.move(time);
                    }
                },
                move: function (time) {
                    this.headPosition.x = Phaser.Math.Wrap(this.headPosition.x + x, 0, weightBlock);
                    this.headPosition.y = Phaser.Math.Wrap(this.headPosition.y + y, 0, hightBlock);

                    Phaser.Actions.ShiftPosition(this.body.getChildren(), this.headPosition.x * block, this.headPosition.y * block, 1, this.tail);



                    var hitBody = Phaser.Actions.GetFirst(this.body.getChildren(), { x: this.head.x, y: this.head.y }, 1);

                    if (hitBody) {

                        console.log('dead');

                        this.alive = false;

                        return false;
                    }
                    else {

                        this.moveTime = time + this.speed;

                        return true;
                    }
                },

                grow: function () {
                    var newPart = this.body.create(this.tail.x, this.tail.y, 'cat');

                    newPart.setOrigin(0);
                },

                collideWithFood: function (food) {
                    if (this.head.x === food.x && this.head.y === food.y) {
                        this.grow();

                        food.eat();


                        if (this.speed > 20 && food.total % 5 === 0) {
                            this.speed -= 5;
                        }

                        return true;
                    }
                    else {
                        return false;
                    }
                },

                updateGrid: function (grid) {
                    this.body.children.each(function (segment) {

                        var bx = segment.x / block;
                        var by = segment.y / block;

                        grid[by][bx] = false;

                    });
                    return grid;
                }

            });

            food = new Love(this, 3, 4);

            cat = new Cat(this, 8, 8);

            this.cursors = this.input.keyboard.addKeys({
                leftKey: Phaser.Input.Keyboard.KeyCodes.LEFT,
                rightKey: Phaser.Input.Keyboard.KeyCodes.RIGHT
            });
            pointer = this.input.activePointer;
            // 計算分數
            this.sorceText = this.add.text(400, 460, `Sorce:${sorceInt}`, { color: '#fff', fontSize: '30px' });
            this.TimeText = this.add.text(60, 460, `Time:${timeInt}`, { color: '#fff', fontSize: '30px' })
            // 計時
            timer = setInterval(() => {
                timeInt++;
                this.TimeText.setText(`Time:${timeInt}`);
            }, 1000);

        },
        update: function (time) {
            // 分數更新 顯示
            this.sorceText.setText(`Sorce:${sorceInt}`);
            // 接分數，最後用來傳入資料庫
            let sorce = sorceInt;
            let tryURL = `?sorce=${sorce}&page=${"again"}`;
            let backURL = `?sorce=${sorce}&page=${"back"}`;
            if (!cat.alive) {
                if (sorceTrue) {
                    // 放呼叫API
                    console.log("HI 我撞到了");
                    sorceTrue = false;
                }
                this.over_title = this.add.image(330, 240, 'over_title');
                this.try_again = this.add.image(330, 270, 'try_again');
                //this.back_menu = this.add.image(500, 300, 'back_menu');
                this.try_again.setInteractive();
                this.try_again.on('pointerdown',
                    () => {
                        //this.scene.start('gamePlay');
                        console.log(tryURL);
                        //window.location.href += tryURL;
                        sorceInt = 0;
                        timeInt = 0;
                        $("#phaser>canvas").remove();
                        game = new Phaser.Game(config);
                    });
                //this.back_menu.setInteractive();
                //this.back_menu.on('pointerdown',
                //    () => {
                //        //this.scene.start('gamePlay');
                //        window.location.href += backURL;
                //        sorceInt = 0;
                //        timeInt = 0;
                //    });
                clearInterval(timer);
                return;
            }
            function key_touch_left() {
                if (x === 1 && y === 0) {
                    // cat.faceUp();
                    x = 0;
                    y = -1;
                    console.log(x, y, "up");
                } else if (x === 0 && y === -1) {
                    // cat.faceLeft();
                    x = -1;
                    y = 0;
                    console.log(x, y, "left");
                } else if (x === -1 && y === 0) {
                    // cat.faceDown();
                    x = 0;
                    y = 1;
                    console.log(x, y, "down");
                } else if (x === 0 && y === 1) {
                    // cat.faceRight();
                    x = 1;
                    y = 0;
                    console.log(x, y, "Right");
                }
            }
            function key_touch_right() {
                if (x === 1 && y === 0) {
                    // cat.faceDown();
                    x = 0;
                    y = 1;
                    console.log(x, y, "up");
                } else if (x === 0 && y === 1) {
                    // cat.faceLeft();
                    x = -1;
                    y = 0;
                    console.log(x, y, "left");
                } else if (x === -1 && y === 0) {
                    // cat.faceUp();
                    x = 0;
                    y = -1;
                    console.log(x, y, "down");
                } else if (x === 0 && y === -1) {
                    // cat.faceRight();
                    x = 1;
                    y = 0;
                    console.log(x, y, "Right");
                }
            }
            //Touch
            if (pointer.leftButtonReleased()) {
                cnt = 0;
            }
            if (pointer.isDown && (pointer.x < 400 && pointer.x > 0)) {
                cnt++;
                if (cnt < 2) {
                    key_touch_left()
                }
            }
            else if (pointer.isDown && (pointer.x < 800 && pointer.x > 400)) {
                cnt++;
                if (cnt < 2) {
                    key_touch_right()
                }
            }

            // 鍵盤
            const { leftKey, rightKey } = this.cursors;
            if (Phaser.Input.Keyboard.JustDown(leftKey)) {
                key_touch_left();
            }
            else if (Phaser.Input.Keyboard.JustDown(rightKey)) {
                key_touch_right();
            }

            if (cat.update(time)) {
                if (cat.collideWithFood(food)) {
                    this.sound.play('eat');
                    repositionFood();
                }
            }

            function repositionFood() {
                var testGrid = [];
                for (var y = 0; y < hightBlock; y++) {
                    testGrid[y] = [];

                    for (var x = 0; x < weightBlock; x++) {
                        testGrid[y][x] = true;
                    }
                }
                cat.updateGrid(testGrid);
                var validLocations = [];
                for (var y = 0; y < hightBlock; y++) {
                    for (var x = 0; x < weightBlock; x++) {
                        if (testGrid[y][x] === true) {
                            validLocations.push({ x: x, y: y });
                        }
                    }
                }
                if (validLocations.length > 0) {
                    var pos = Phaser.Math.RND.pick(validLocations);
                    food.setPosition(pos.x * block, pos.y * block);
                    return true;
                }
                else {
                    return false;
                }
            }
        }
    }


    var config = {
        type: Phaser.AUTO,
        width: w,
        height: h,
        backgroundColor: '#5B5B5B',
        parent: 'phaser',
        input: {
            activePointers: 1
        },
        scene: [gameStar, gamePlay]
    };

    var game = new Phaser.Game(config);
}