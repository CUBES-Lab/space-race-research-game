document.getElementById('textInput1').addEventListener('input', function () {
    document.getElementById('submitText1').disabled = this.value.trim() === '';
});

document.getElementById('submitText1').addEventListener('click', function () {
    const text1 = document.getElementById('textInput1').value.trim();
    if (text1 !== '') {
        document.getElementById('textInput2').disabled = false;
        document.getElementById('textInput2').classList.remove('disabled');
        document.getElementById('submitText2').disabled = false;
        document.getElementById('submitText2').classList.remove('disabled');
    }
});

document.getElementById('textInput2').addEventListener('input', function () {
    document.getElementById('submitText2').disabled = this.value.trim() === '';
});

document.getElementById('submitText2').addEventListener('click', function () {
    const text2 = document.getElementById('textInput2').value.trim();
    if (text2 !== '') {
        document.getElementById('countButton').disabled = false;
        document.getElementById('countButton').classList.remove('disabled');
    }
});

document.getElementById('countButton').addEventListener('click', checkUUIDCount);

function getUUIDFromURL() {
    const urlParams = new URLSearchParams(window.location.search);
    return urlParams.get('uuid');
}

function checkUUIDCount() {
    const uuid = getUUIDFromURL();
    if (!uuid) {
        alert('UUID not found in URL.');
        return;
    }

    // Example URL to the Google Sheets API
    const sheetURL = `https://docs.google.com/spreadsheets/d/1-ZbloFuaSxy8jxOQQlK3O6scjx5R4C6rc7tYs_ZYBKM/edit?gid=673635246#gid=673635246`;

    fetch(sheetURL)
        .then(response => response.json())
        .then(data => {
            if (data.count >= 10) {
                document.getElementById('postSurveySection').classList.remove('hidden');
                document.getElementById('postSurveyLink').classList.remove('disabled');
                document.getElementById('postSurveyLink').removeAttribute('disabled');
                document.getElementById('postSurveyText').disabled = false;
                document.getElementById('postSurveyText').classList.remove('disabled');
            } else {
                alert(`UUID count is ${data.count}. It needs to be at least 10 to access the post-survey page.`);
            }
        })
        .catch(error => {
            console.error('Error fetching UUID count:', error);
        });
}
