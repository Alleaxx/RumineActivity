// ==UserScript==
// @name         Логгирование постов на странице
// @namespace    http://tampermonkey.net/
// @version      0.1
// @description  Добавляет кнопку для копирования ID и дат всех постов на странице форума ru-minecraft.ru для уже дальнейшей вставки в файл
// @author       Alleaxx
// @match        http://*/*
// @icon         data:image/gif;base64,R0lGODlhAQABAAAAACH5BAEKAAEALAAAAAABAAEAAAICTAEAOw==
// @grant        none
// ==/UserScript==

(function () {
    'use strict';

    function fallbackCopyTextToClipboard(text) {
        var textArea = document.createElement("textarea");
        textArea.value = text;

        // Avoid scrolling to bottom
        textArea.style.top = "0";
        textArea.style.left = "0";
        textArea.style.position = "fixed";

        document.body.appendChild(textArea);
        textArea.focus();
        textArea.select();

        try {
            var successful = document.execCommand('copy');
            var msg = successful ? 'successful' : 'unsuccessful';
            console.log('Fallback: Copying text command was ' + msg);
        } catch (err) {
            console.error('Fallback: Oops, unable to copy', err);
        }

        document.body.removeChild(textArea);
    }
    function copyTextToClipboard(text) {
        if (!navigator.clipboard) {
            fallbackCopyTextToClipboard(text);
            return;
        }
        navigator.clipboard.writeText(text).then(function () {
            console.log('Async: Copying to clipboard was successful!');
        }, function (err) {
            console.error('Async: Could not copy text: ', err);
        });
    }


    let postsInfo = document.querySelectorAll(".forum-topicMsgDate");
    let postsInfoArr = [];
    postsInfo.forEach(info => {
        let dateText = info.querySelector("a").nextSibling.textContent.trim();
        let postID = info.querySelector("input").getAttribute("value").trim();
        if (postID != 1239156) {
            console.log(postID + " " + dateText);
            postsInfoArr.push(postID + " " + dateText)
        }
    });

    let copyButton = document.createElement("button");
    let copyButtonText = document.createElement("span");
    copyButtonText.textContent = "Скопировать посты";
    copyButton.style.float = "right";
    copyButtonText.style.width = "auto";
    copyButtonText.style.color = "#638aa9";
    copyButton.appendChild(copyButtonText);
    const desTreeElement = document.getElementById("desTtee");
    copyButton.onclick = () => {
        copyTextToClipboard(postsInfoArr.join("\n") + "\n");
        copyButtonText.textContent = "Скопировано!";
    };

    desTreeElement.appendChild(copyButton);
})();