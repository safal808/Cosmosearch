<!DOCTYPE html>
<html lang="en">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Search Engine</title>
    <link rel="stylesheet" href="https://fonts.googleapis.com/css2?family=Inter:wght@300;400;500;600;700&display=swap">
    <style>
        body {
            font-family: 'Inter', sans-serif;
            margin: 0;
            padding: 0;
            margin-left: 20px;
            background-color: #f4f4f4;
        }

        h1 {
            margin-top: 20px;
        }

        #searchForm {
            margin-top: 20px;
        }

        input[type="text"] {
            padding: 8px;
            border-radius: 5px;
            border: 1px solid #ccc;
            width: 300px;
            font-family: 'Inter', sans-serif;
        }

        button {
            padding: 8px 15px;
            border-radius: 5px;
            border: none;
            background-color: #007bff;
            color: white;
            cursor: pointer;
            font-family: 'Inter', sans-serif;
        }

        button:hover {
            background-color: #0056b3;
        }

        #searchResultsContainer {
            display: none;
            margin-top: 20px;
            width: 85%;
            padding: 10px;
            background-color: #fff;
            border-radius: 5px;
            box-shadow: 0 0 5px rgba(0, 0, 0, 0.1);
        }

        .searchItem {
            padding: 10px;
            border-bottom: 1px solid #ccc;
            margin-left: 20px;
            margin-bottom: 10px;
        }

        .searchItem a {
            text-decoration: none;
            color: #1a0dab;
            font-size: 14px;
            display: block;
            margin-bottom: 5px;
        }

        .searchItem a:hover {
            text-decoration: underline;
        }

        .searchItem h2 {
            font-size: 18px;
            margin: 5px 0;
        }

        .searchItem p {
            font-size: 14px;
            margin: 5px 0;
            color: #545454;
        }
    </style>
</head>

<body>
    <h1>Cosmo Search</h1>
    <form id="searchForm">
        <input type="text" id="searchQuery" name="searchQuery" placeholder="Enter your search query..." />
        <button type="submit">Search</button>
    </form>
    <div id="searchResultsContainer">
        <div id="searchResults">
        </div>
    </div>

    <script>
        document.getElementById('searchForm').addEventListener('submit', function (event) {
            event.preventDefault();

            var searchQuery = document.getElementById('searchQuery').value;

            var xhr = new XMLHttpRequest();
            xhr.open('GET', '/search?word=' + encodeURIComponent(searchQuery), true);
            xhr.onreadystatechange = function () {
                if (xhr.readyState === XMLHttpRequest.DONE) {
                    if (xhr.status === 200) {
                        var results = JSON.parse(xhr.responseText);
                        displayResults(results);
                    } else {
                        displayError('Error occurred while searching.');
                    }
                }
            };
            xhr.send();
        });

        function displayResults(results) {
            var searchResultsDiv = document.getElementById('searchResults');
            searchResultsDiv.innerHTML = '';

            var searchResultsContainer = document.getElementById('searchResultsContainer');
            searchResultsContainer.style.display = 'block';

            if (results.length === 0) {
                searchResultsDiv.innerHTML = '<p>No results found.</p>';
            } else {
                results.forEach(function (result) {
                    var searchItem = document.createElement('div');
                    searchItem.classList.add('searchItem');

                    var link = document.createElement('a');
                    link.href = result.Url;
                    link.textContent = result.Url;
                    link.target = "_blank"; // Open link in new tab
                    searchItem.appendChild(link);

                    var titleLink = document.createElement('a');
                    titleLink.href = result.Url;
                    titleLink.textContent = result.Title;
                    titleLink.style.fontSize = '18px'; // Larger font size for title
                    titleLink.style.color = '#333'; // Darker color for title
                    var title = document.createElement('h2');
                    title.appendChild(titleLink);
                    searchItem.appendChild(title);

                    var description = document.createElement('p');
                    description.textContent = result.Description;
                    searchItem.appendChild(description);

                    searchResultsDiv.appendChild(searchItem);
                });
            }
        }

        function displayError(message) {
            var searchResultsDiv = document.getElementById('searchResults');
            searchResultsDiv.innerHTML = '<p>' + message + '</p>';

            var searchResultsContainer = document.getElementById('searchResultsContainer');
            searchResultsContainer.style.display = 'block';
        }
    </script>
</body>

</html>
