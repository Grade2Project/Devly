'use strict';

const tinderContainer = document.querySelector('.tinder');
let allCards = document.querySelectorAll('.tinder__cards');
const nope = document.getElementById('nope');
const love = document.getElementById('love');
const template = document.getElementById('card_template');
setHammerOnSingleCard(template);

function refreshCards() {
    allCards = document.querySelectorAll('.tinder__cards');
    initCards();
}
function initCards(card, index) {
    const newCards = document.querySelectorAll('.tinder__card:not(.removed)');

    newCards.forEach(function (card, index) {
        card.style.zIndex = allCards.length - index;
        card.style.transform = 'scale(' + (20 - index) / 20 + ') translateY(-' + 30 * index + 'px)';
        card.style.opacity = (10 - index) / 10;
    });

    tinderContainer.classList.add('loaded');
}

function setHammerOnSingleCard(el) {
    let hammertime = new Hammer(el);

    hammertime.on('pan', function (event) {
        if (event.target.className === 'extra-info__button') return;
        el.classList.add('moving');
    });

    hammertime.on('pan', function (event) {
        if (event.target.className === 'extra-info__button') return;
        if (event.deltaX === 0) return;
        if (event.center.x === 0 && event.center.y === 0) return;
        tinderContainer.classList.toggle('tinder_love', event.deltaX > 0);
        tinderContainer.classList.toggle('tinder_nope', event.deltaX < 0);

        let xMulti = event.deltaX * 0.03;
        let yMulti = event.deltaY / 80;
        let rotate = xMulti * yMulti;

        event.target.style.transform = 'translate(' + event.deltaX + 'px, ' + event.deltaY + 'px) rotate(' + rotate + 'deg)';
    });

    hammertime.on('panend', function (event) {
        if (event.target.className === 'extra-info__button') return;
        el.classList.remove('moving');
        tinderContainer.classList.remove('tinder_love');
        tinderContainer.classList.remove('tinder_nope');

        let moveOutWidth = document.body.clientWidth;
        let keep = Math.abs(event.deltaX) < 80 || Math.abs(event.velocityX) < 0.5;

        event.target.classList.toggle('removed', !keep);


        if (keep) {
            event.target.style.transform = '';
        } else {
            let endX = Math.max(Math.abs(event.velocityX) * moveOutWidth, moveOutWidth);
            let toX = event.deltaX > 0 ? endX : -endX;
            if (event.deltaX > 0) suggestLike();
            else suggestDislike();
            cardHandler.appendCardToDoc();

            let endY = Math.abs(event.velocityY) * moveOutWidth;
            let toY = event.deltaY > 0 ? endY : -endY;

            let xMulti = event.deltaX * 0.03;
            let yMulti = event.deltaY / 80;
            let rotate = xMulti * yMulti;

            event.target.style.transform = 'translate(' + toX + 'px, ' + (toY + event.deltaY) + 'px) rotate(' + rotate + 'deg)';
            initCards();

        }
    });
}

function createButtonListener(love) {
    return function (event) {
        const cards = document.querySelectorAll('.tinder__card:not(.removed)');
        const moveOutWidth = document.body.clientWidth * 1.5;

        if (!cards.length) return false;

        let card = cards[0];

        card.classList.add('removed');

        if (love) {
            card.style.transform = 'translate(' + moveOutWidth + 'px, -100px) rotate(-30deg)';
            suggestLike();
        } else {
            card.style.transform = 'translate(-' + moveOutWidth + 'px, -100px) rotate(30deg)';
            suggestDislike();
        }

        cardHandler.appendCardToDoc();

        event.preventDefault();
    };
}

function suggestLike() {
    console.log('like');
}

function suggestDislike() {
    console.log('dislike');
}

const nopeListener = createButtonListener(false);
const loveListener = createButtonListener(true);

nope.addEventListener('click', nopeListener);
love.addEventListener('click', loveListener);
