Bakalaureusetöö <b>"Mõned failide salvestamise disainimustrid SQL-andmebaasi kasutavate andmebaasirakenduste jaoks" </b>
raames tehtud rakendused disainilahenduste võrdlemiseks


Repositoorium sisaldab nelja erinevat piltide salvestamise disainilahenduse projekti: 


* <b>BLOB</b> - piltide salvestamine andmebaasi suurobjektidena
* <b>Bytea</b> - piltide salvestamine andmebaasi bytea veergu
* <b>Cloud</b> - piltide salvestamine Amazon S3 korvi ja andmebaasis hoitakse viidet pildi asukohale
* <b>Filesystem</b> - piltide salvestamine failisüsteemi ja andmebaasis hoitakse viidet pildi asukohale

Erle Maido, 2021





<i>Abiks:</i>

Migratsioonid

~~~
dotnet ef migrations add Initial
dotnet ef database update
dotnet ef database drop
~~~

Docker image ehitamiseks

~~~
docker build -t erlemaido/bytea-products .
docker build -t erlemaido/blob-products .
docker build -t erlemaido/cloud-products .
docker build -t erlemaido/filesystem-products .
~~~

Dockeri image pushimiseks 

~~~
docker push erlemaido/bytea-products
docker push erlemaido/blob-products
docker push erlemaido/cloud-products
docker push erlemaido/filesystem-products
~~~
