﻿function RungameShow() {
    Swal.fire({
        showConfirmButton: false,
        html: `<div id="myrungame"></div>`,
        showCloseButton: true
    })
    Rungame();
}

function Rungame() {


    const w = 812;
    const h = 375;
    let sorceInt = 0;

    // 遊戲開始
    const gameStart = {
        key: 'gameStart',
        preload: function () {

            this.load.image('bg1', '/Content/images/mengo/bg/bg1_1.png');
            this.load.image('bg2', '/Content/images/mengo/bg/bg2_1.png');
            this.load.image('bg3', '/Content/images/mengo/bg/bg3_1.png');
            this.load.image('bg4', '/Content/images/mengo/bg/bg4_1.png');
            this.load.image('footer', '/Content/images/mengo/bg/footer_1.png');

            this.load.image('logo', '/Content/images/mengo/ui/txt-title_1.png');
            this.load.image('startBtn', '/Content/images/mengo/ui/btn-press-start_1.png');
            this.load.image('playerEnd', '/Content/images/mengo/ui/player-end_1.png');

        },
        // 加入遊戲物件與相關設定
        create: function () {
            game.scale.pageAlignHorizontally = true;
            game.scale.pageAlignVertically = true;

            // 呈現場景
            this.bg4 = this.add.tileSprite(w / 2, h / 2, w, h, 'bg4');
            this.bg3 = this.add.tileSprite(w / 2, h / 2, w, h, 'bg3');
            this.bg2 = this.add.tileSprite(w / 2, h / 2, w, h, 'bg2');
            this.bg1 = this.add.tileSprite(w / 2, h / 2, w, h, 'bg1');
            this.footer = this.add.tileSprite(w / 2, 405, w, 90, 'footer');

            this.playerEnd = this.add.image(w / 2, 310, 'playerEnd');
            this.playerEnd.setScale(0.5);
            this.logo = this.add.image(w / 2, h / 2 - 100, 'logo');
            this.logo.setScale(0.5);
            this.startBtn = this.add.image(w / 2, h / 2, 'startBtn');
            this.startBtn.setScale(0.7);

            this.startBtn.setInteractive();

            this.startBtn.on('pointerdown', () => {
                this.scene.start('gamePlay');
            });
        },
        // 遊戲狀態更新
        update: function () {
            this.bg3.tilePositionX += 3;
            this.bg2.tilePositionX += 2;
            this.bg1.tilePositionX += 4;
            this.footer.tilePositionX += 4;
        }
    }

    const getRandom = (max, min) => {
        return Math.floor(Math.random() * (max - min + 1)) + min;
    };

    const gamePlay = {
        key: 'gamePlay',
        // 載入資源
        preload: function () {
            // 圖片
            // 背景
            this.load.image('bg1', '/Content/images/mengo/bg/bg1_1.png');
            this.load.image('bg2', '/Content/images/mengo/bg/bg2_1.png');
            this.load.image('bg3', '/Content/images/mengo/bg/bg3_1.png');
            this.load.image('bg4', '/Content/images/mengo/bg/bg4_1.png');
            this.load.image('footer', '/Content/images/mengo/bg/footer_1.png');
            // 載入音效
            this.load.audio('jump12', '/Content/images/gameAudio/jump12.mp3');
            this.load.audio('touch', '/Content/images/gameAudio/touch.mp3');
            // jumpBtn
            // this.load.image('btnJump', '../images/ui/btn-jump.png');
            // monster
            this.load.image('rock1', '/Content/images/mengo/showman.png');
            this.load.image('rock2', '/Content/images/mengo/showman.png');
            this.load.image('rock3', '/Content/images/mengo/showman.png');
            // 遊戲結束後的標題
            this.load.image('gameover', '/Content/images/mengo/ui/txt-game-over_1.png');
            this.load.image('tryAgainBtn', '/Content/images/mengo/ui/btn-try-again_1.png');
            this.load.image('playAgainBtn', '/Content/images/mengo/ui/btn-play-again_1.png');
            // 腳色動畫
            this.load.spritesheet('user', '/Content/images/mengo/f_player.png', { frameWidth: 144, frameHeight: 120 });

            // 時間
            this.timeInt = 0;
            // 分數 

            // 遊戲是否停止
            this.gameStop = false;
            // 是否可以跳躍
            this.iskeyJump = true;
            // 存放所有怪物實體
            this.monsterArr = [];
            // 存放所有怪物實體2    
            this.monsterArr2 = [];
            // 怪物索引 1
            this.masIdx = 0;
            // 怪物索引2
            this.masIdx2 = 1;
            // 速度
            this.bgSpeed = 1.3;
        },
        // 加入遊戲物件與相關設定
        create: function () {
            // 將圖片背景放置
            this.bg4 = this.add.tileSprite(w / 2, h / 2, w, h, 'bg4');
            this.bg3 = this.add.tileSprite(w / 2, h / 2, w, h, 'bg3');
            this.bg2 = this.add.tileSprite(w / 2, h / 2, w, h, 'bg2');
            this.bg1 = this.add.tileSprite(w / 2, h / 2, w, h, 'bg1');
            this.footer = this.add.tileSprite(w / 2, 405, w, 90, 'footer');

            // 設定腳色sprite
            this.player = this.physics.add.sprite(105, 105, 'user');
            // 縮小腳色尺寸
            this.player.setScale(0.7);
            // 腳色邊界限制
            this.player.setCollideWorldBounds(true);
            // 設定腳色碰撞範圍
            this.player.setSize(105, 115);
            // 設定人物與地板物理接觸
            this.physics.add.collider(this.player, this.footer);

            // 將地板加入物理效果
            this.physics.add.existing(this.footer);
            // 設定地板不因加入物理重力影響
            this.footer.body.immovable = true;
            // 設定物件位置旋轉是否受影響
            this.footer.body.moves = false;

            // 加入怪物物理效果
            const addPhysics = GameObject => {
                this.physics.add.existing(GameObject);
                GameObject.body.immovable = true;
                GameObject.body.moves = false;
            }

            // 走路 一般狀態
            this.anims.create({
                key: 'run',
                frames: this.anims.generateFrameNumbers('user', { start: 0, end: 1 }),
                frameRate: 5,
                repeat: -1
            });
            // 跳
            this.anims.create({
                key: 'jump',
                frames: this.anims.generateFrameNumbers('user', { start: 2, end: 2 }),
                frameRate: 5,
                repeat: -1
            });
            // 飛起
            this.anims.create({
                key: 'landing',
                frames: this.anims.generateFrameNumbers('user', { start: 3, end: 3 }),
                frameRate: 5,
                repeat: -1
            });
            // deal
            this.anims.create({
                key: 'deal',
                frames: this.anims.generateFrameNumbers('user', { start: 4, end: 4 }),
                frameRate: 5,
                repeat: -1
            });

            // 怪物的座標資訊
            const masPos = [
                { name: 'rock1', x: w + 200, y: 320, w: 160, h: 83 },
                { name: 'rock2', x: w + 200, y: 320, w: 160, h: 83 },
                { name: 'rock3', x: w + 200, y: 320, w: 160, h: 83 },
            ]

            // 撞到一次寫一次if判斷
            var myAPItrue = true;

            //碰撞到後停止遊戲
            const hittest = (player, rock) => {
                let gameURL = `?sorce=${sorceInt}`;
                if (myAPItrue) {
                    console.log("撞到後會顯示嗎?")
                    // 呼叫API
                    myAPItrue = false;
                }
                this.sound.play('touch');
                this.gameStop = true;
                this.player.setSize(110, 100, 0);
                // deal圖
                this.player.anims.play('deal', true);
                clearInterval(timer);
                this.gameover = this.add.image(w / 2, h / 2 - 40, 'gameover');
                this.gameover.setScale(0.8);
                this.tryAgainBtn = this.add.image(w / 2, h / 2 + 30, 'tryAgainBtn');
                this.tryAgainBtn.setScale(0.6);
                this.tryAgainBtn.setInteractive();
                this.tryAgainBtn.on('pointerdown', () => {
                    // 這邊是按下按鈕會回傳
                    myAPItrue = true;
                    // 刪除原本的
                    $("#myrungame>canvas").remove();
                    // 新建一個新的
                    sorceInt = 0;
                    game = new Phaser.Game(config);
                    //window.location.href += gameURL;
                });

                this.sound.stopAll();
            }

            // 產生怪物
            for (let i = 0; i < 10; i++) {
                let BoolIdx = getRandom(2, 0);
                let BoolIdx2 = getRandom(2, 0);
                this['rock' + i] = this.add.tileSprite(masPos[BoolIdx].x, masPos[BoolIdx].y, masPos[BoolIdx].w, masPos[BoolIdx].h, masPos[BoolIdx].name);
                this['rockB' + i] = this.add.tileSprite(masPos[BoolIdx2].x, masPos[BoolIdx2].y, masPos[BoolIdx2].w, masPos[BoolIdx2].h, masPos[BoolIdx2].name);
                this.monsterArr.push(this['rock' + i]);
                this.monsterArr2.push(this['rockB' + i]);
                this['rock' + i].setScale(0.4);
                this['rockB' + i].setScale(0.4);
                this['rock' + i].setSize(170, 200, 0);
                this['rockB' + i].setSize(170, 200, 0);
                addPhysics(this['rock' + i]);
                addPhysics(this['rockB' + i]);
                this.physics.add.collider(this.player, this['rock' + i], hittest);
                this.physics.add.collider(this.player, this['rockB' + i], hittest);
            }

            // 時間倒數
            this.TimeText = this.add.text(50, 0, `Time:${this.timeInt}`, { color: '#fff', fontSize: '30px' })
            // 計算分數
            this.sorceText = this.add.text(340, 0, `Sorce:${sorceInt}`, { color: '#fff', fontSize: '30px' })
            // 計時
            let timer = setInterval(() => {
                this.timeInt++;
                sorceInt += 100;
                //console.log("HI" + sorceInt)
                this.TimeText.setText(`Time:${this.timeInt}`);
                this.sorceText.setText(`Sorce:${sorceInt}`);
                if ((this.timeInt % 10) == 0) {
                    // 每10秒加速度
                    this.bgSpeed += 0.2;
                }
            }, 1000);

            // this.player.anims.play('run', true);
        },
        // 遊戲狀態更新
        update: function () {
            // 當Stop為true 則停止update
            if (this.gameStop) return;

            // 場景移動
            this.bg3.tilePositionX += 3 * this.bgSpeed;
            this.bg2.tilePositionX += 2 * this.bgSpeed;
            this.bg1.tilePositionX += 4 * this.bgSpeed;
            this.footer.tilePositionX += 4 * this.bgSpeed;
            // 怪物移動
            this.monsterArr[this.masIdx].x -= 6 * this.bgSpeed;
            // 當時間來到50秒就增加一個怪物
            if (this.timeInt > 50) {
                this.monsterArr2[this.masIdx2].x -= 6 * this.bgSpeed;
            }

            // 檢測怪物是否超出邊界然後返回
            for (let i = 0; i < this.monsterArr.length; i++) {
                if (this.monsterArr[i].x <= -100) {
                    this.monsterArr[i].x = w + 200;
                    this.masIdx = getRandom(this.monsterArr.length - 1, 0);
                }
                if (this.monsterArr2[i].x <= -100) {
                    this.monsterArr2[i].x = w + getRandom(400, 200);
                    this.masIdx2 = getRandom(this.monsterArr2.length - 1, 0);
                }
            }

            // 滑鼠點擊
            let pointer = this.input.activePointer;
            // 鍵盤輸入
            let keyboard = this.input.keyboard.createCursorKeys();
            // 當接觸地面時才可以起跳
            if (this.player.body.blocked.down && (keyboard.up.isDown || pointer.isDown)) {
                this.sound.play('jump12');
                this.player.anims.play('jump', true);
                // 跳躍高度
                this.player.setVelocityY(-600);
                // console.log('up');
            } else {
                // 回到地面時 回復走路狀態
                if (this.player.body.blocked.down) {
                    this.player.anims.play('run', true);
                }
            }
        }
    }

    // 設定canvas
    const config = {
        type: Phaser.AUTO,
        width: w,
        height: h,
        parent: 'myrungame',
        // 加入物理引擎
        physics: {
            default: 'arcade',
            arcade: {
                // 重力值
                gravity: {
                    y: 1100
                },
                // 查看物件碰撞範圍
                // debug: true
            },
        },
        // 場景 兩個 start play
        scene: [gameStart, gamePlay]
    }
    let game = new Phaser.Game(config);
}