import { JsonDiff } from "./json-diff.js";

const hiddenClass = 'hidden';
const visibleClass = 'visible';

const showJsonDiffPage = () => {
    const jsonDiffBlock = document.querySelector('.content');
    jsonDiffBlock.classList.add(visibleClass);
    jsonDiffBlock.classList.remove(hiddenClass);
}

const unshowJsonDiffPage = () => {
    const jsonDiffBlock = document.querySelector('.content');
    jsonDiffBlock.classList.add(hiddenClass);
    jsonDiffBlock.classList.remove(visibleClass);
}

const form = document.querySelector('.main-form');
const textareaOld = document.querySelector('#oldJson');
const textareaNew = document.querySelector('#newJson');
const resultBlock = document.querySelector('.result');

form.addEventListener('submit', (event) => {
    event.preventDefault();
    const oldValue = JSON.parse(textareaOld.value);
    const newValue = JSON.parse(textareaNew.value);

    const result = JsonDiff.create(oldValue, newValue);
    const resultStr = JSON.stringify(result, undefined, 2);
    resultBlock.innerHTML = `<pre>${resultStr}</pre>`;
    const classList = resultBlock.classList;
    const resultVisibleClass = 'result__visible';

    if (!classList.contains(resultVisibleClass)) {
        classList.add(resultVisibleClass);
    }
});

export const JsonDiffPage = { showJsonDiffPage, unshowJsonDiffPage };