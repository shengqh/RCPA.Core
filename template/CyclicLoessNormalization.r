#predefine_start

outputdir<-"H:/shengquanhu/projects/rcpa/TurboRaw2Mgf/iTRAQ4/mascot/summary"
inputfile<-"8-deisotopic-top10-removeitraq-range.noredundant.I114I115.isobaric.20110915_iTRAQ_4plex_GK_6ug_Exp_1.tsv"
outputfile<-"8-deisotopic-top10-removeitraq-range.noredundant.I114I115.isobaric.20110915_iTRAQ_4plex_GK_6ug_Exp_1.norm.tsv"
missingvalue<-0.1

#predefine_end

# Written by Ashoka D. Polpitiya
# for the Translational Genomics Research Institute (TGen, Phoenix, AZ)
# Copyright 2010, Translational Genomics Research Institute
# E-mail: ashoka@tgen.org
# Website: http://inferno4proteomics.googlecode.com
# -------------------------------------------------------------------------
#
# Licensed under the Apache License, Version 2.0; you may not use this file except
# in compliance with the License.  You may obtain a copy of the License at
# http://www.apache.org/licenses/LICENSE-2.0
#
# -------------------------------------------------------------------------

# Do the Loess normalization on replicates
loess_normalize <- function(Data, replicates, span=0.2, plotflag=FALSE,
                            reference=1,folder="C:/")
{
  Nreps <- unique(as.vector(t(replicates)))
  
  for (i in 1:length(Nreps)) # for each unique sample
  {
    idx <- which(replicates == Nreps[i])
    dataset <- Data[,idx] # extract data for sample i with all the replicates
    
    if (length(idx) > 1) # do LOESS
    {
      fittedData <- doLOESSreplicates(dataset, sp=span, plotflag,
                                      folder=folder, reference=reference)
      Data[,idx] <- fittedData
    }
  }# for
  return(Data)
}# function

#------------------------------------------------------------------
doLOESSreplicates <- function(dataset, sp=0.2, plotflag=FALSE,
                              folder="C:/", reference=1)
{
  fittedData <- dataset
  header <- colnames(dataset)
  
  if (reference == 1) # pick the first dataset
  {
    print("LOESS: First set")
    refset <- 1
    indexSet <- dataset[,refset]
    set2normalize <- 1:dim(dataset)[2]
    set2normalize <- set2normalize[set2normalize != refset]
    idxSetName <- header[refset]
  }
  if (reference == 2) # median
  {
    print("LOESS: Median")
    indexSet <- apply(dataset,1,"median",na.rm=TRUE)
    set2normalize <- 1:dim(dataset)[2]
    idxSetName <- "Median"
  }
  if (reference == 3) # least missing
  {
    print("LOESS: Least missing")
    xx <- 1:dim(dataset)[2]
    missTotal <- colSums(is.na(dataset))
    refset <- xx[missTotal==min(missTotal,na.rm=TRUE)]
    refset <- refset[1]
    indexSet <- dataset[,refset]
    set2normalize <- 1:dim(dataset)[2]
    set2normalize <- set2normalize[set2normalize != refset]
    idxSetName <- header[refset]
  }
  
  for (cycle in set2normalize)
  {
    #browser()
    matchSet <- dataset[,cycle]
    Mean <- (indexSet + matchSet)/2
    Diff <- indexSet - matchSet
    #LOESS<-loess(Diff~Mean, family="gaussian", span=sp) # based on positions both numeric
    LOESS<-loess(Diff~Mean, family="symmetric", span=sp) # based on positions both numeric
    FIT <- LOESS$fit
    
    # handle missing values
    positionsbothnumeric <- !is.na(matchSet+indexSet)
    positionsbothnumeric[is.na(positionsbothnumeric)] <- FALSE
    missingindex <- is.na(indexSet) # missing in indexSet
    missing <- matchSet[missingindex] # corresponding entries in match Set
    nas <- array(NA,dim=length(missing))
    presentInMatchSet <- missing[!is.na(missing)] # present in matchSet but missing in indexSet
    doPredict = (length(presentInMatchSet) > 0)
    if (doPredict)
    {
      predict(LOESS,presentInMatchSet) -> nas[!is.na(missing)] # estimate them
    }
    fittedMatchSet <- matchSet
    fittedMatchSet[positionsbothnumeric] <- matchSet[positionsbothnumeric] + FIT
    if (doPredict)
    {
      fittedMatchSet[missingindex] <- matchSet[missingindex] + nas
    }
    else
      fittedMatchSet[missingindex] <- matchSet[missingindex]
    
    fittedMean <- (indexSet + fittedMatchSet)/2
    fittedDiff <- indexSet - fittedMatchSet
    
    fittedData[,cycle] <- fittedMatchSet
    
    if (plotflag)
    {
      #browser()
      pic1<-paste(folder, header[cycle],"_Loess.png",sep="")
      pic2<-paste(folder, header[cycle],"_LoessFitted.png",sep="")
      png(filename=pic1, width=1024, height=768, pointsize=12, bg="white", units="px")
      #require(Cairo)
      #CairoPNG(filename=pic1,width=IMGwidth,height=IMGheight,pointsize=FNTsize,bg="white",res=600)
      plot(Mean,Diff, pch=21,bg="green", xlab="MEAN (A)",
           ylab="DIFF (M)", main=paste("M-A Plot (",idxSetName," - ",
                                       header[cycle],")",sep=""))
      points(na.omit(Mean),LOESS$fit,col=2,pch=20)
      abline(h=0, col=1, lwd=1)
      dev.off()
      
      png(filename=pic2, width=1024, height=768, pointsize=12, bg="white", units="px")
      plot(fittedMean,fittedDiff, pch=21,bg="purple", xlab="MEAN (A)",
           ylab="DIFF (M)", main=paste("M-A Plot after Loess (",
                                       idxSetName," - ", header[cycle],")",sep=""))
      abline(h=0, col=1)
      dev.off()
    } # if
  } # for
  return(fittedData)
}

# Written by Quanhu Sheng
# for Vanderbilt Center for Quantitative Science
# Copyright 2015
# E-mail: shengqh@gmail.com
# Website: https://github.com/shengqh/RCPA.Tools
# -------------------------------------------------------------------------
#
# Licensed under the Apache License, Version 2.0; you may not use this file except
# in compliance with the License.  You may obtain a copy of the License at
# http://www.apache.org/licenses/LICENSE-2.0
#
# -------------------------------------------------------------------------

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
        mtext(side=3, paste0("mean(ratio)=", sprintf("%.2f", mean(m)), "; sd=", sprintf("%.2f", sd(m))))
        abline(a=0, b=0)
      }
    }
  }
  close.screen()
  dev.off()
}

library(limma)

setwd(outputdir)

data<-read.table(inputfile, header=T, sep="\t", row.names=1)

hasNA<-length(table(data<=missingvalue))==2
if(hasNA){
  pre_png<-paste0(outputfile, ".PreInfernoRDNLoessNorm.png")
  post_png<-paste0(outputfile, ".PostInfernoRDNLoessNorm.png")
}else{
  pre_png<-paste0(outputfile, ".PreCyclicLoessNorm.png")
  post_png<-paste0(outputfile, ".PostCyclicLoessNorm.png")
}
drawimage(data, pre_png)

data[data==0]<-NA

#loess non-linear regression
logdata<-log(data)

if(hasNA){
  log_loess_data<-loess_normalize(logdata,replicates=rep(1,ncol(logdata)),reference=2)
}else{
  log_loess_data<-normalizeCyclicLoess(logdata, method="affy")
}
loess_data<-exp(log_loess_data)

colnames(loess_data)<-colnames(data)
rownames(loess_data)<-rownames(data)

loess_data[is.na(loess_data)]<-missingvalue

loess_data<-cbind(rownames(loess_data), loess_data)
colnames(loess_data)[1]<-"FileScan"
write.table(loess_data, file=outputfile, sep="\t", row.names=F, quote=F)

drawimage(loess_data[,c(2:ncol(loess_data))], post_png)
