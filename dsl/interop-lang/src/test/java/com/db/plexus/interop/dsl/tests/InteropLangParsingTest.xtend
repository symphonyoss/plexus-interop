/**
 * Copyright 2017 Plexus Interop Deutsche Bank AG
 * SPDX-License-Identifier: Apache-2.0
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
/*
 * generated by Xtext 2.12.0
 */
package com.db.plexus.interop.dsl.tests

import com.google.inject.Inject
import org.eclipse.xtext.testing.InjectWith
import org.eclipse.xtext.testing.XtextRunner
import org.junit.Assert
import org.junit.Test
import org.junit.runner.RunWith
import org.eclipse.emf.ecore.resource.ResourceSet
import org.eclipse.xtext.EcoreUtil2
import org.eclipse.emf.common.util.URI
import org.eclipse.emf.ecore.resource.Resource
import org.eclipse.xtext.validation.IResourceValidator
import org.eclipse.xtext.validation.CheckMode
import org.eclipse.xtext.util.CancelIndicator
import com.db.plexus.interop.dsl.protobuf.ProtoLangConfig

@RunWith(XtextRunner)
@InjectWith(InteropLangInjectorProvider)
class InteropLangParsingTest {
	
	@Inject
	ResourceSet rs
	
	@Inject
	IResourceValidator validator
	
	@Inject
	ProtoLangConfig protoLangConfig 
		
	@Test
	def void loadModel() {
		val uri = URI.createURI(ClassLoader.getSystemClassLoader().getResource("com/db/plexus/interop/dsl/tests/example1.interop").toURI().toString())
		val baseUri = uri.trimSegments(1).appendSegment("")
		protoLangConfig.baseURIs.add(baseUri)
		System.out.println("Parsing " + uri)
		Assert.assertEquals(0, rs.resources.length)
		rs.getResource(uri, true)
		EcoreUtil2.resolveAll(rs);
		val allResources = rs.resources		
		for (r : allResources) {
			validateResource(r)
		}								
		Assert.assertEquals(5, allResources.length)	
	}
	
	@Test
	def void ParseFilesWithPredefinedResources() {
		val uri = URI.createURI(ClassLoader.getSystemClassLoader().getResource("com/db/plexus/interop/dsl/tests/example3.interop").toURI().toString())
		System.out.println("Parsing " + uri)
		Assert.assertEquals(0, rs.resources.length)
		rs.getResource(uri, true)
		EcoreUtil2.resolveAll(rs);
		val allResources = rs.resources		
		for (r : allResources) {
			validateResource(r)
		}								
		Assert.assertEquals(4, allResources.length)	
	}
	
	def validateResource(Resource r) {		
		System.out.println("Validating " + r.URI)
		val issues = validator.validate(r, CheckMode.ALL, CancelIndicator.NullImpl)		
		for (issue : issues) {			
			System.err.println(issue)			
		}
		System.out.flush()
		System.err.flush()		
		Assert.assertEquals(0, issues.length)		
	}	
}
