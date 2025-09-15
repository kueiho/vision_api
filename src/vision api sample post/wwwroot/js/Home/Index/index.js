let functions = $('#functions-data').data('functions');

function renderFunction(idx) {
    let func = functions[idx];
    $('#functionList .list-group-item').removeClass('active').eq(idx).addClass('active');
    $('#requestJson').val(JSON.stringify(func.request, null, 2));
    $('#responseJson, #responseTime').text('');
    if (func.preview) {
        $('#previewImg').attr('src', func.preview).show();
    } else {
        $('#previewImg').hide().attr('src', '');
    }
    $('#fileInput').val('');
}

$(function () {
    $('#functionList').on('click', '.list-group-item', function () {
        renderFunction($(this).data('idx'));
    });

    $('#fileInput').on('change', function (e) {
        let file = e.target.files[0];
        if (!file) {
            $('#previewImg').hide().attr('src', '');
            return;
        }
        let reader = new FileReader();
        reader.onload = e => $('#previewImg').attr('src', e.target.result).show();
        reader.readAsDataURL(file);
    });

    $('#previewImg').on('click', function () {
        let src = $(this).attr('src');
        if (src && src.startsWith('data:image/')) {
            // 轉成 Blob
            let arr = src.split(',');
            let mime = arr[0].match(/:(.*?);/)[1];
            let bstr = atob(arr[1]);
            let n = bstr.length;
            let u8arr = new Uint8Array(n);
            while (n--) {
                u8arr[n] = bstr.charCodeAt(n);
            }
            let blob = new Blob([u8arr], { type: mime });
            let url = URL.createObjectURL(blob);
            window.open(url, '_blank');
        } else {
            window.open(src, '_blank');
        }
    });

    $('#uploadForm').on('submit', async function (e) {
        e.preventDefault();
        let reqText = $('#requestJson').val();
        try {
            let reqObj = JSON.parse(reqText);
            let content = $('#previewImg').attr('src');

            if (Array.isArray(reqObj.requests)) {
                for (let req of reqObj.requests) {
                    if (req.image) {
                        // 如果是 https:// 開頭
                        if (content && content.startsWith('https://')) {
                            // fetch 轉 base64
                            req.image.source = { imageUri: content };
                            if (req.image.content) delete req.image.content;
                        }
                        // 如果是 base64（包含 ;base64,不論檔案型態）
                        else if (content && content.includes(';base64,')) {
                            req.image.content = content.split(';base64,')[1];
                            if (req.image.source) delete req.image.source;
                        }
                        // 如果是 / 開頭（本地相對路徑）
                        else if (content && content.startsWith('/')) {
                            // 本地相對路徑 檔案 轉 base64
                            req.image.content = await fetchImageAsBase64(content);
                            if (req.image.source) delete req.image.source;
                        }
                    }
                }
            }

            $('#requestJson').val(JSON.stringify(reqObj, null, 2));
            sendRequest();
        } catch (err) {
            $('#responseTime').text(err.message || String(err));
        }
    });

    function sendRequest() {
        let reqText = $('#requestJson').val();
        $.ajax({
            url: '/Home/Test',
            type: 'POST',
            data: { requestJson: reqText },
            success: function (result) {
                $('#responseJson').text(JSON.stringify(JSON.parse(result.response), null, 2));
                $('#responseTime').text(formatDateTime(new Date()));
            },
            error: function () {
                $('#responseTime').text('');
                alert('上傳失敗，請重試。');
            }
        });
    }

    function formatDateTime(date) {
        const pad = (n, z = 2) => ('00' + n).slice(-z);
        return date.getFullYear() + '/' +
            pad(date.getMonth() + 1) + '/' +
            pad(date.getDate()) + ' ' +
            pad(date.getHours()) + ':' +
            pad(date.getMinutes()) + ':' +
            pad(date.getSeconds());
    }

    async function fetchImageAsBase64(url) {
        const response = await fetch(url);
        const blob = await response.blob();
        return new Promise((resolve, reject) => {
            const reader = new FileReader();
            reader.onloadend = function () {
                const base64 = reader.result.split(',')[1];
                resolve(base64);
            };
            reader.onerror = reject;
            reader.readAsDataURL(blob);
        });
    }
});