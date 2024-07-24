document.addEventListener('DOMContentLoaded', () => {
    // 读取文件并处理
    fetch('/国家.txt')
        .then(response => response.text())
        .then(data => {
            // 按行分割文件内容
            const lines = data.split('\n').map(line => line.trim()).filter(line => line);

            // 获取下拉框元素
            const dropdown = document.getElementById('dropdown');

            // 将每一行添加为下拉框的选项
            lines.forEach(line => {
                const option = document.createElement('option');
                option.value = line;
                option.textContent = line;
                dropdown.appendChild(option);
            });
        })
        .catch(error => {
            console.error('Error fetching or processing the file:', error);
        });


    addEvt2();
    addEvt3();
});


function addEvt2() {

    // Listen for changes to the first dropdown
    dropdown.addEventListener('change', (event) => {
        const selectedValue = event.target.value;

        // Clear the second dropdown
        //  dropdown2.innerHTML = '<option value="">Select a city</option>';
        dropdown2.innerHTML = ''
        dropdown3.innerHTML = ''
        if (selectedValue) {
            fetch('/' + selectedValue + '城市列表.txt')
                .then(response => response.text())
                .then(data => {
                    // Split the file content into lines
                    const cities = data.split('\n').map(line => line.trim()).filter(line => line);

                    // Populate the second dropdown with city options
                    cities.forEach(city => {
                        const option = document.createElement('option');
                        option.value = city;
                        option.textContent = city;
                        dropdown2.appendChild(option);
                    });
                })
                .catch(error => {
                    console.error('Error fetching or processing the file:', error);
                });
        }
    });

}

function addEvt3() {

    // Listen for changes to the first dropdown
    dropdown2.addEventListener('change', (event) => {
        const selectedValue = event.target.value;

        // Clear the second dropdown
        dropdown3.innerHTML = '<option value="">Select a 园区</option>';
        //  dropdown3.innerHTML = ''
        if (selectedValue) {
            fetch('/' + selectedValue + '园区.txt')
                .then(response => response.text())
                .then(data => {
                    // Split the file content into lines
                    const cities = data.split('\n').map(line => line.trim()).filter(line => line);

                    // Populate the second dropdown with city options
                    cities.forEach(city => {
                        const option = document.createElement('option');
                        option.value = city;
                        option.textContent = city;
                        dropdown3.appendChild(option);
                    });
                })
                .catch(error => {
                    console.error('Error fetching or processing the file:', error);
                });
        }
    });

}