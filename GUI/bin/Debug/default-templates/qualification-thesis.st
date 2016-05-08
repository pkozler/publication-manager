<!DOCTYPE html>
<!-- výchozí šablona pro export publikace do HTML dokumentu pro umístění na webové stránky -->
<html lang="cs">
    <head>
        <title>$title$</title>
        <meta charset="UTF-8">
        <meta name="viewport" content="width=device-width, initial-scale=1.0">
		<link rel="stylesheet" href="http://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap.min.css">
		<script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.0/jquery.min.js"></script>
		<script src="http://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/js/bootstrap.min.js"></script>
    </head>
    <body>
        <div class="container">
			<div class="page-header">
				<h1>$title$</h1>
			</div>
			<dl class="dl-horizontal">
				<dt>Autor:</dt>
				<dd>$author$</dd>
				<dt>Rok vydání:</dt>
				<dd>$year$</dd>
				<dt>Typ publikace:</dt>
				<dd>$type$</dd>
			</dl>
			<dl class="dl-horizontal"> <!-- bibliografické údaje pro kvalifikační práci -->
				<dt>Místo vytvoření:</dt>
				<dd>$address$</dd>
				<dt>Škola:</dt>
				<dd>$school$</dd>
				<dt>Typ kvalifikační práce:</dt>
				<dd>$type$</dd>
			</dl> <!-- konec bibliografických údajů -->
			<div class="text-justify">
				<p>$text$</p>
			</div>
		</div>
    </body>
</html>