FROM ubuntu:18.04

RUN apt-get update
RUN apt-get install -y sudo curl wget git unzip locales

RUN locale-gen en_US.UTF-8
ENV LANG en_US.UTF-8 

# Install java
RUN wget -nv --show-progress --progress=bar:force:noscroll https://download.java.net/java/GA/jdk13.0.1/cec27d702aa74d5a8630c65ae61e4305/9/GPL/openjdk-13.0.1_linux-x64_bin.tar.gz
RUN sudo chmod 775 openjdk-13.0.1_linux-x64_bin.tar.gz
RUN sudo tar xfz openjdk-13.0.1_linux-x64_bin.tar.gz
# Export JAVA_HOME
ENV JAVA_HOME /jdk-13.0.1
ENV PATH $PATH:$JAVA_HOME/bin

# Install nodejs
RUN curl -sL https://deb.nodesource.com/setup_12.x | bash -
RUN sudo apt-get install -y nodejs

# Install dotnetcore
RUN wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
RUN sudo dpkg -i packages-microsoft-prod.deb
RUN sudo apt-get update; \
  sudo apt-get install -y apt-transport-https && \
  sudo apt-get update && \
  sudo apt-get install -y dotnet-sdk-3.1

# Install elastic
RUN wget -nv --show-progress --progress=bar:force:noscroll https://artifacts.elastic.co/downloads/elasticsearch/elasticsearch-7.4.0-linux-x86_64.tar.gz
RUN sudo chmod 775 elasticsearch-7.4.0-linux-x86_64.tar.gz
RUN tar xfz elasticsearch-7.4.0-linux-x86_64.tar.gz 

# Export ELASTIC_HOME
ENV ELASTIC_HOME /elasticsearch-7.4.0
ENV PATH $PATH:$ELASTIC_HOME/bin

# Configure elastic
RUN printf "\ndiscovery.type: single-node\
\nnetwork.host: 0.0.0.0" >> elasticsearch-7.4.0/config/elasticsearch.yml

# Serbian-analyzer
RUN wget -nv --show-progress --progress=bar:force:noscroll https://github.com/markomartonosi/udd06/archive/plugin-update.zip
RUN sudo chmod 775 plugin-update.zip 
RUN unzip plugin-update.zip 

# Build serbian-analyzer
RUN   echo "// UTF-8 \n\
      compileJava.options.encoding = 'UTF-8' \n\
      javadoc.options.encoding = 'UTF-8' \n" >> /udd06-plugin-update/build.gradle
RUN cd /udd06-plugin-update && ./gradlew clean build -x integTestCluster#wait
# Install serbian-analyzer
RUN sudo chmod 775 /udd06-plugin-update/build/distributions/serbian-analyzer-1.0-SNAPSHOT.zip
RUN elasticsearch-plugin install file:/udd06-plugin-update/build/distributions/serbian-analyzer-1.0-SNAPSHOT.zip
# Install ingest-attachment
RUN elasticsearch-plugin install --batch ingest-attachment
# Create elasticsearch user
RUN sudo groupadd esUser
RUN sudo useradd esUser -g esUser -p esUser
RUN chown -R esUser:esUser /elasticsearch-7.4.0

# Clone repo 
RUN git clone https://github.com/dobrica/udd

# Build server
# RUN dotnet build /udd/service/udd.sln
RUN cd '/udd/service' && dotnet publish -c Release -o published
RUN cd '/udd/service/published' && mkdir Database
RUN cp '/udd/service/udd/Database/InitDb.sql' '/udd/service/published/Database/'
RUN cp -r '/udd/service/udd/files' '/udd/service/published/'
RUN chmod 777 -R '/udd/service/published'
RUN chown -R esUser:esUser /udd
ENV ASPNETCORE_URLS "https://0.0.0.0:44370"

# Install angular
RUN cd "/udd/client" && npm install -g @angular/cli && npm update

# Prepare startup script
RUN echo "#!/bin/bash\n\
echo 'starting elastic...'\n\
su esUser -c '/elasticsearch-7.4.0/bin/elasticsearch -d'\n\
echo 'starting dotnet...'\n\
cd '/udd/service/published/' && dotnet udd.dll &\n\ 
echo 'starting angular...'\n\
cd '/udd/client/' && ng serve --host 0.0.0.0\n" >> /startup.sh
RUN chmod u+x startup.sh

EXPOSE 4200
EXPOSE 9200
EXPOSE 44370

ENTRYPOINT ["./startup.sh"]