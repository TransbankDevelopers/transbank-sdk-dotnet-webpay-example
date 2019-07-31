FROM mono:6.0.0.313

RUN apt-get update
RUN apt-get install -y mono-xsp4 asp.net-examples

COPY . /app
WORKDIR /app
RUN nuget restore TransbankWebpayExample.sln
RUN msbuild /p:Configuration=Debug TransbankWebpayExample.sln
WORKDIR /app/WebpayASPNetExample

EXPOSE 8080
CMD ["xsp4", "--address", "0.0.0.0", "--port", "8080", "--nonstop"]
