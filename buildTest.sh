docker build --pull --target test -t discordbot-test .
docker run --rm -v ${pwd}/TestResults:/source/tests/TestResults discordbot-test