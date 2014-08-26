#predefine_start

outputdir<-"H:/shengquanhu/projects/rcpa/TurboRaw2Mgf/iTRAQ4/mascot/summary"
inputfile<-"8-deisotopic-top10-removeitraq-range.noredundant.I114I115.isobaric.20110915_iTRAQ_4plex_GK_6ug_Exp_1.tsv"
outputfile<-"8-deisotopic-top10-removeitraq-range.noredundant.I114I115.isobaric.20110915_iTRAQ_4plex_GK_6ug_Exp_1.norm.tsv"

#predefine_end

library(limma)

setwd(outputdir)

data<-read.table(inputfile, header=T, sep="\t", row.names=1)

drawimage<-function(data, pngFilename){
  dd<-data.frame(data)
  png(pngFilename, width=1000 * ncol(dd), height=1000*ncol(dd), res=300)
  split.screen(c(ncol(dd),ncol(dd)))
  id<-0
  cols<-rainbow(ncol(dd))
  
  for(i in c(1:ncol(dd))){
    for(j in c(1:ncol(dd))){
      id<-id+1
      screen(id)
      if(i == j){
        dl<-list()
        maxy<-0
        for(k in c(1:ncol(dd))){
          if(k != i){
            notZero<-dd[,i] > 0 & dd[,k] > 0
            xx<-dd[notZero,i]
            yy<-dd[notZero,k]
            r<-log2(yy/xx)
            d<-density(r)
            dl[[colnames(dd)[k]]]<-d
            maxy<-max(maxy, max(d$y))
          }  
        }
        maxy<-floor(maxy) + 1
        s<-0
        for(k in c(1:ncol(dd))){
          if(k != i){
            s<-s+1
            if(s == 1){
              par(mar=c(2,2,3,2), new=TRUE)
              plot(dl[[colnames(dd)[k]]], xlim=c(-2,2), ylim=c(0,maxy), xlab="", col=cols[k], main=colnames(dd)[i])
            }else{
              lines(dl[[colnames(dd)[k]]], col=cols[k])
            }
          }
        }
      }else{
        notZero<-dd[,i] > 0 & dd[,j] > 0
        xx<-dd[notZero,i]
        yy<-dd[notZero,j]
        m<-log2(yy/xx)
        a<-log2((yy+xx)/2)
        par(mar=c(2,2,3,2), new=TRUE)
        plot(x=a, y=m, col=cols[j],xlab=",ylab=",main=paste0(colnames(dd)[j], "/", colnames(dd)[i]), cex=0.1, pch=20, xlim=c(10,30), ylim=c(-5,5))
        abline(a=0, b=0)
      }
    }
  }
  close.screen()
  dev.off()
}

beforepng<-paste0(outputfile, ".PreCyclicLoessNorm.png")
drawimage(data, beforepng)

#loess non-linear regression
loess_data<-normalizeCyclicLoess(data, iterations=2)
colnames(loess_data)<-colnames(data)
rownames(loess_data)<-rownames(data)

afterpng<-paste0(outputfile, ".PostCyclicLoessNorm.png")
drawimage(loess_data, afterpng)

loess_data<-cbind(rownames(loess_data), loess_data)
colnames(loess_data)[1]<-"FileScan"

write.table(loess_data, file=outputfile, sep="\t", row.names=F, quote=F)
