<!DOCTYPE html>
<!-- výchozí šablona pro export publikace do HTML dokumentu pro umístění na webové stránky -->
<html lang="cs">
    <head>
        <title>$title$ - technická zpráva č. $id$</title>
        <meta charset="UTF-8">
        <meta name="viewport" content="width=device-width, initial-scale=1.0">
		<link rel="stylesheet" href="http://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap.min.css">
		<script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.0/jquery.min.js"></script>
		<script src="http://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/js/bootstrap.min.js"></script>
    </head>
    <body>
    	<div class="container">
	        <div class="row">
	        	<div class="col-xs-12 page-header">
					<h1>$title$</h1>
				</div>
	        </div>
			<div class="row">
				<div class="col-xs-6">
					<dl class="dl-horizontal">
						<dt>Autoři:</dt>
						<dd>$authors$</dd>
						<dt>Rok vydání:</dt>
						<dd>$year$</dd>
						<dt>Typ publikace:</dt>
						<dd>$type$</dd>
					</dl>
				</div>
				<div class="col-xs-6">
					<dl class="dl-horizontal"> <!-- bibliografické údaje pro technickou zprávu -->
						<dt>Místo vytvoření:</dt>
						<dd>$address$</dd>
						<dt>Instituce:</dt>
						<dd>$institution$</dd>
						<dt>Označení:</dt>
						<dd>$number$</dd>
					</dl> <!-- konec bibliografických údajů -->
				</div>
			</div>
			<div class="row">
				<div class="col-xs-12 text-justify">
					<hr/>
						<p>$text$</p>
					<hr/>
				</div>
			</div>
			<div class="row">
				<div class="col-xs-12 text-right">
					<p class="small text-muted">Vygenerováno programem Seznam publikací (Verze 2.0)<br/>Copyright (c) Petr Kozler, 2016<br/></p>
				</div>
			</div>
		</div>
    </body>
</html>