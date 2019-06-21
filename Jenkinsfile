pipeline {
  environment {
    registry = "fnikolovski/backgroundworker"
    registryCredential = 'dockerhub'
    //dockerImage = ''
  }
  agent any

  stages {
    stage('Cloning Git') {
      steps {
        git 'https://github.com/filip-nikolovskipro/integrationservice.git'
      }
    }
   // stage('Build') {
   //    steps {
   //      sh "docker build -t integrationservice:B${BUILD_NUMBER} -f Dockerfile ."
   //   }
   // }
    // stage('Test') {
    //   steps {
    //     sh 'npm test'
    //   }
    // }
    stage('Building image') {
      steps{
        script {
          dockerImage = docker.build registry + ":$BUILD_NUMBER"
        }
      }
    }
    stage('Deploy Image') {
      steps{
         script {
            docker.withRegistry( '', registryCredential ) {
            dockerImage.push()
          }
        }
      }
    }
    stage('Remove Unused docker image') {
      steps{
        sh "docker rmi $registry:$BUILD_NUMBER"
      }
    }
  }
}
